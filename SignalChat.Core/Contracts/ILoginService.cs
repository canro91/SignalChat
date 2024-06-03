namespace SignalChat.Core.Contracts
{
    public interface ILoginService
    {
        Task<string?> LoginAsync(string username, string plainTextPassword);
    }
}
