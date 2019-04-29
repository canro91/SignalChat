using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using System;

namespace SignalChat.Core.Tasks
{
    public class SendMessageService : ISendMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public SendMessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public bool Send(string username, string message)
        {
            var toSend = new Message
            {
                ID = Guid.NewGuid(),
                Username = username,
                Body = message,
                DeliveredAt = DateTimeOffset.Now
            };
            _messageRepository.Save(message: toSend);
            return false;
        }
    }
}
