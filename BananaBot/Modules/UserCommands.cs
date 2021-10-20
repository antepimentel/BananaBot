using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BananaBot.Modules
{
    public class UserCommands : ModuleBase
    {

        private JSONHandler data = new JSONHandler();

        [Command("adduser")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task AddUser(IUser user, string emoteStr)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (Global.userList.ContainsKey(user.Id))
            {
                Global.userList.Remove(user.Id);
            }
            Global.userList.Add(user.Id, emoteStr);

            data.updateJSON(Global.userList);

            Emoji emote = new Emoji(emoteStr);
            await ReplyAsync("Added "+user.Username+ " with "+emote);
        }

        [Command("rmuser")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task RmUser(IUser user)
        {
            if (Global.userList.ContainsKey(user.Id))
            {
                Global.userList.Remove(user.Id);
            }

            data.updateJSON(Global.userList);
            await ReplyAsync("Removed " + user.Username);
        }

        [Command("clearall")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task ClearAll()
        {
            Global.userList.Clear();
            data.updateJSON(Global.userList);
            await ReplyAsync("All users cleared");
        }
    }
}
