using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BananaBot
{
    class JSONHandler
    {

        private static string datapath = "data.json";

        public async void updateJSON(Hashtable table)
        {
            //JSONHandler.resetFile();

            List<User> _data = new List<User>();

            foreach(UInt64 key in table.Keys)
            {
                _data.Add(new User(key, (string)table[key]));
            }

            string json = JsonConvert.SerializeObject(_data.ToArray());

            using (StreamWriter writer = File.CreateText(datapath))
            {
                await writer.WriteAsync(json);
            }

            //File.WriteAllText(datapath, json);
        }

        public async Task<Task> readJSON()
        {
            //JSONHandler.checkFile();
            Hashtable table = new Hashtable();

            using(StreamReader reader = File.OpenText(datapath))
            {
                string json = await reader.ReadToEndAsync();
                if (json.Length != 0)
                {
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
                    foreach (User user in users)
                    {
                        table.Add(user.Id, user.Emote);
                    }
                }
                
                Global.userList = table;
                return Task.CompletedTask;
            }

        }

        private static void checkFile()
        {
            if (!File.Exists(datapath))
            {
                File.CreateText(datapath);
            }
        }

        private static void resetFile()
        {
            if (File.Exists(datapath))
            {
                File.Delete(datapath);
                File.CreateText(datapath);
            }
        }
    }
}
