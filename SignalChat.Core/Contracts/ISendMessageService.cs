namespace SignalChat.Core.Contracts
{
    public interface ISendMessageService
    {
        bool Send(string username, string message);
    }
}
