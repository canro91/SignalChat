using System;

namespace SignalChat.Core
{
    public class User
    {
        public Guid ID { get; set; }
        public string Username { get; set; }
        public string SaltedPassword { get; set; }
    }
}
