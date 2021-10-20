using System;
using System.Collections.Generic;
using System.Text;

namespace BananaBot
{
    class User
    {

        public UInt64 Id { get; set; }
        public string Emote { get; set; }

        public User(UInt64 _id, string _emote)
        {
            Id = _id;
            Emote = _emote;
        }
    }
}
