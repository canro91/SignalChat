namespace SignalChat.Core.Domain;

public class User
{
    public required string Username { get; set; }
    public required string SaltedPassword { get; set; }
}
