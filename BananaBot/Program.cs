using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BananaBot
{
    class Program
    {

        private DiscordSocketClient _client;
        private IConfiguration _config;

        public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandler;
            _client.MessageReceived += ReactHandler;
            _client.Log += Log;

            var _builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory).AddJsonFile(path: "config.json");
            _config = _builder.Build();

            var token = File.ReadAllText("token.txt");

            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            await _client.LoginAsync(Discord.TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(Discord.LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
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
