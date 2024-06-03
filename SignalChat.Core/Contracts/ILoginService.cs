namespace SignalChat.Core.Contracts
{
    public interface ILoginService
    {
        string? Login(string username, string plainTextPassword);
    }
}
