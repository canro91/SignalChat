namespace SignalChat.Core.Contracts
{
    public interface ISendMessageService
    {
        Task<bool> SendAsync(string username, string message);
    }
}
