using SignalChat.Bot.Contracts;

namespace SignalChat.Bot.Services
{
    public class OnlineStockService : IStockService
    {
        public byte[] FindStockQuote(string stockCode)
        {
            var request = new RestSharp.RestRequest($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");

            var client = new RestSharp.RestClient();
            return client.DownloadData(request);
        }
    }
}
