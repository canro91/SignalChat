using RestSharp;
using SignalChat.Bot.Contracts;

namespace SignalChat.Bot.Services
{
    public class OnlineStockService : IStockService
    {
        public byte[] FindStockQuote(string stockCode)
        {
            var request = new RestRequest($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");

            var client = new RestClient();
            return client.DownloadData(request);
        }
    }
}
