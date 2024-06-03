namespace SignalChat.Models
{
    public class MessageViewModel
    {
        public string? Username { get; set; }

        public string? Body { get; set; }

        public DateTimeOffset DeliveredAt { get; set; }
    }
}
