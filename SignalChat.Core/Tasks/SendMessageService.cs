using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using System.Text.RegularExpressions;

namespace SignalChat.Core.Tasks;

public class SendMessageService : ISendMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IBotService _botService;

    public SendMessageService(IMessageRepository messageRepository,
                              IBotService botService)
    {
        _messageRepository = messageRepository;
        _botService = botService;
    }

    public async Task<bool> SendAsync(string username, string message)
    {
        var (isCommand, stockCode) = IsStockCommand(message);
        if (isCommand)
        {
            await _botService.QueryAndSendAsync(stockCode!);

            return true;
        }
        else
        {
            var toSend = new Message
            {
                Username = username,
                Body = message,
                DeliveredAt = DateTimeOffset.Now
            };
            await _messageRepository.SaveAsync(message: toSend);

            return false;
        }
    }

    private static readonly Regex StockCommand = new Regex(@"^\/stock=(?<StockCode>.+)$", RegexOptions.Compiled);

    private static (bool isCommand, string? stockCode) IsStockCommand(string message)
    {
        var matches = StockCommand.Match(message);
        return matches.Success
                ? (true, matches.Groups["StockCode"].Value)
                : (false, null);
    }
}