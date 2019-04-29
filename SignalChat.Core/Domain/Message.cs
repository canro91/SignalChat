using System;

namespace SignalChat.Core.Domain
{
    public class Message
    {
        public Guid ID { get; set; }
        public string Username { get; set; }
        public string Body { get; set; }
        public DateTimeOffset DeliveredAt { get; set; }
    }
}