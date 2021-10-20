using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Discord.Commands;
using BananaBot.Services;

namespace BananaBot
{
    class Program
    {

        private DiscordSocketClient _client;
        private IConfiguration _config;

        public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        public Program()
        {
            // create the configuration
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");

            // build the configuration and assign to _config          
            _config = _builder.Build();
        }

        public async Task MainAsync()
        {


            // call ConfigureServices to create the ServiceCollection/Provider for passing around the services
            using (var services = ConfigureServices())
            {
                // get the client and assign to client 
                // you get the services vwia GetRequiredService<T>
                var client = services.GetRequiredService<DiscordSocketClient>();
                _client = client;

                // setup logging and the ready event
                client.Log += LogAsync;
                client.Ready += ReadyAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                // this is where we get the Token value from the configuration file, and start the bot
                await client.LoginAsync(TokenType.Bot, _config["token"]);
                await client.StartAsync();

                // we get the CommandHandler class here and call the InitializeAsync method to start things up for the CommandHandler service
                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(-1);
            }
        }

        // this method handles the ServiceCollection creation/configuration, and builds out the service provider we can call on later
        private ServiceProvider ConfigureServices()
        {
            // this returns a ServiceProvider that is used later to call for those services
            // we can add types we have access to here, hence adding the new using statement:
            // using csharpi.Services;
            // the config we build is also added, which comes in handy for setting the command prefix!
            return new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"Connected as -> [] :)");
            return Task.CompletedTask;
        }

        private Task CommandHandler(SocketMessage message)
        {
            string prefix = "!b";
            string command = "";
            string param = "";
            int cmdLength = -1;

            if (!message.Content.StartsWith(prefix))
                return Task.CompletedTask;

            if (message.Author.IsBot)
                return Task.CompletedTask;

            if (message.Content.Contains(' '))
                cmdLength = message.Content.IndexOf(' ');
            else
                cmdLength = message.Content.Length;

            //cmdLength -= 1;
            //command = message.Content.Substring(prefix.Length, cmdLength); // Does not work, made custom substring function
            command = Substring(message.Content, prefix.Length, cmdLength);
            param = Substring(message.Content, message.Content.LastIndexOf(' '), message.Content.Length);

            if (command.Equals("beep"))
                message.Channel.SendMessageAsync("boop");
            else if (command.Equals("adduser"))
                CmdUser.handleAddUser(message.MentionedUsers, param.Trim());
            else if (command.Equals("rmuser"))
                CmdUser.handleRmUser(message.MentionedUsers);
            else if (command.Equals("clear"))
                CmdUser.handleClear();
            return Task.CompletedTask;
        }

        private Task ReactHandler(SocketMessage message)
        {
            try
            {
                if (CmdUser.userTable.ContainsKey(message.Author.Id))
                {
                    Console.WriteLine(message.Content);
                    //var emote = Emote.Parse((string)CmdAddUser.userTable[message.Author.Id]);
                    Emoji emote = new Emoji((string)CmdUser.userTable[message.Author.Id]);
                    //Emoji emote = new Emoji("👍");
                    message.AddReactionAsync(emote);
                }
                return Task.CompletedTask;
            } catch(Exception e)
            {
                message.Channel.SendMessageAsync("Whoops something went wrong");
                Console.WriteLine(e.Message);
                return Task.CompletedTask;
            }
        }

        private string Substring(string input, int start, int end)
        {
            string result = "";

            if (input.Length < end)
                return input;
            else if (start < 0 || start > end)
                return input;
            else
            {
                for(int i = start; i < end; i++)
                {
                    result += input.ToCharArray()[i];
                }
            }
            return result;
        }
    }
}
