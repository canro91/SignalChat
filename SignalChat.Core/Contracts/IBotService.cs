namespace SignalChat.Core.Contracts
{
    public interface IBotService
    {
        Task QueryAndSendAsync(string stockCode);
    }
}
