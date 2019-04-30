using CsvHelper;
using Foundatio.Queues;
using SignalChat.Bot.Contracts;
using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SignalChat.Bot
{
    public class BotService : IBotService
    {
        private readonly IStockService _stockService;
        private readonly IQueue<Message> _messageQueue;

        public BotService(IStockService stockService, IQueue<Message> messageQueue)
        {
            _stockService = stockService;
            _messageQueue = messageQueue;
        }

        public async Task QueryAndSend(string stockCode)
        {
            byte[] bytes = _stockService.FindStockQuote(stockCode);

            using (var stream = new MemoryStream(bytes))
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.Delimiter = ",";
                var records = csv.GetRecords<StockQuoteCsv>();

                foreach (var r in records)
                {
                    var m = new Message
                    {
                        Body = string.Format(SN.Message, r.Symbol, r.Close),
                        DeliveredAt = DateTimeOffset.Now,
                        Username = SN.Bot
                    };
                    await _messageQueue.EnqueueAsync(m);
                }
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
