namespace SignalChat.Core.Domain
{
    public class User
    {
        public Guid ID { get; set; }
        public required string Username { get; set; }
        public required string SaltedPassword { get; set; }
    }
}
