using CsvHelper;
using Foundatio.Queues;
using SignalChat.Bot.Contracts;
using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using System.Globalization;

namespace SignalChat.Bot.Services;

public class BotService : IBotService
{
    private readonly IStockService _stockService;
    private readonly IQueue<Message> _messageQueue;

    public BotService(IStockService stockService,
                      IQueue<Message> messageQueue)
    {
        _stockService = stockService;
        _messageQueue = messageQueue;
    }

    public async Task QueryAndSendAsync(string stockCode)
    {
        byte[] bytes = await _stockService.FindStockQuoteAsync(stockCode);

        using var stream = new MemoryStream(bytes);
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<StockQuoteCsv>();

        foreach (var r in records)
        {
            var symbol = r.Symbol;
            var closePrice = r.Close;

            var body = closePrice == StockQuoteCsv.NotAvailable
                            ? string.Format(SN.Unavailable, symbol)
                            : string.Format(SN.Message, symbol, closePrice);

            var m = new Message
            {
                Body = body,
                DeliveredAt = DateTimeOffset.Now,
                Username = SN.Bot
            };
            await _messageQueue.EnqueueAsync(m);
        }
    }
}

class StockQuoteCsv
{
    public const string NotAvailable = "N/D";

    public string? Symbol { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }
    public string? Open { get; set; }
    public string? High { get; set; }
    public string? Low { get; set; }
    public string? Close { get; set; }
    public string? Volume { get; set; }
}
