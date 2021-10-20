using Discord.WebSocket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BananaBot
{
    class CmdUser
    {
        string ducky = "151703773402759169";
        public static Hashtable userTable = new Hashtable();

        public static Task handleAddUser(IReadOnlyCollection<SocketUser> users, string param)
        {
            //Console.WriteLine(param);
            foreach(SocketUser user in users){

                if (userTable.ContainsKey(user.Id))
                {
                    userTable.Remove(user.Id);
                }

                userTable.Add(user.Id, param);
            }
            Console.WriteLine("Users added " + userTable.Count);
            return Task.CompletedTask;
        }

        public static Task handleRmUser(IReadOnlyCollection<SocketUser> users)
        {
            //Console.WriteLine(param);
            foreach (SocketUser user in users)
            {

                if (userTable.ContainsKey(user.Id))
                {
                    userTable.Remove(user.Id);
                }

            }
            Console.WriteLine("Users removed " + userTable.Count);
            return Task.CompletedTask;
        }

        public static Task handleClear()
        {
            userTable.Clear();
            Console.WriteLine("Users removed " + userTable.Count);
            return Task.CompletedTask;
        }

       
    }
}
