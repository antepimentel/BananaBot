using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BananaBot.Modules
{
    public class UserCommands : ModuleBase
    {

        [Command("adduser")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task addUser()
        {
            string msg = Context.Message.Content;
            string param = Util.Substring(msg, msg.LastIndexOf(' '), msg.Length);
            IReadOnlyCollection<ulong> users = Context.Message.MentionedUserIds;
            foreach (ulong id in users)
            {
                if (Global.userList.ContainsKey(id))
                {
                    Global.userList.Remove(id);
                }
                Global.userList.Add(id, param);
            }
            
            await ReplyAsync("Added "+users.Count);
        }

        [Command("rmuser")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task rmUser()
        {
            string msg = Context.Message.Content;
            string param = Util.Substring(msg, msg.LastIndexOf(' '), msg.Length);
            IReadOnlyCollection<ulong> users = Context.Message.MentionedUserIds;
            foreach (ulong id in users)
            {
                if (Global.userList.ContainsKey(id))
                {
                    Global.userList.Remove(id);
                }
            }

            await ReplyAsync("Removed " + users.Count);
        }

        [Command("clearall")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task clearAll()
        {
            Global.userList.Clear();

            await ReplyAsync("All users cleared");
        }
    }
}
