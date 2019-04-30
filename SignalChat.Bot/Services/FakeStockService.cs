using SignalChat.Bot.Contracts;
using System.Text;

namespace SignalChat.Bot.Services
{
    public class FakeStockService : IStockService
    {
        public byte[] FindStockQuote(string stockCode)
        {
            return Encoding.UTF8.GetBytes(@"Symbol,Date,Time,Open,High,Low,Close,Volume
AAPL.US,2019-04-26,22:00:20,204.9,205,202.12,204.3,18649102");
        }
    }
}
