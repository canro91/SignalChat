namespace SignalChat.Core.Domain
{
    public class Message
    {
        public required string Username { get; set; }
        public required string Body { get; set; }
        public DateTimeOffset DeliveredAt { get; set; }
    }
}