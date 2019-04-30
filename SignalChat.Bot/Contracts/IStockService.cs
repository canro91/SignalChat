namespace SignalChat.Bot.Contracts
{
    public interface IStockService
    {
        byte[] FindStockQuote(string stockCode);
    }
}
