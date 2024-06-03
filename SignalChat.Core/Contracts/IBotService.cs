namespace SignalChat.Core.Contracts
{
    public interface IBotService
    {
        Task QueryAndSend(string stockCode);
    }
}
