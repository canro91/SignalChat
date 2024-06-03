using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using System.Text.RegularExpressions;

namespace SignalChat.Core.Tasks
{
    public class SendMessageService : ISendMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IBotService _botService;

        public SendMessageService(IMessageRepository messageRepository, IBotService botService)
        {
            _messageRepository = messageRepository;
            _botService = botService;
        }

        public async Task<bool> SendAsync(string username, string message)
        {
            var (isCommand, stockCode) = IsACommand(message);
            if (isCommand)
            {
                _botService.QueryAndSend(stockCode);

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
        private (bool isCommand, string? stockCode)  IsACommand(string message)
        {
            var matches = StockCommand.Match(message);
            return matches.Success ? (true, matches.Groups["StockCode"].Value)
                                   : (false, null);
        }
    }
}
