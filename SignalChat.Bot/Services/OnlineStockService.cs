using RestSharp;
using SignalChat.Bot.Contracts;

namespace SignalChat.Bot.Services
{
    public class OnlineStockService : IStockService
    {
        public async Task<byte[]> FindStockQuoteAsync(string stockCode)
        {
            var request = new RestRequest($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");

            var client = new RestClient();
            return await client.DownloadDataAsync(request)!;
        }
    }
}
