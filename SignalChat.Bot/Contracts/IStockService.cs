namespace SignalChat.Bot.Contracts;

public interface IStockService
{
    Task<byte[]> FindStockQuoteAsync(string stockCode);
}
