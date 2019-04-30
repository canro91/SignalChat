using CsvHelper;
using SignalChat.Bot.Contracts;
using SignalChat.Core.Contracts;
using System.IO;

namespace SignalChat.Bot
{
    public class BotService : IBotService
    {
        private readonly IStockService _stockService;

        public BotService(IStockService stockService)
        {
            _stockService = stockService;
        }

        public void QueryAndSend(string stockCode)
        {
            byte[] bytes = _stockService.FindStockQuote(stockCode);

            using (var stream = new MemoryStream(bytes))
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.Delimiter = ",";
                var record = csv.GetRecord<StockQuoteCsv>();

                var message = string.Format(SN.Message, stockCode.ToUpper(), record.Symbol, record.Close);
            }
        }
    }

    class StockQuoteCsv
    {
        public string Symbol { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public string Volume { get; set; }
    }
}
