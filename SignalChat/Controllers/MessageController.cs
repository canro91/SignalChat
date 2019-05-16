using SignalChat.Core.Contracts;
using SignalChat.Filters;
using SignalChat.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SignalChat.Controllers
{
    [Authorize]
    [ValidateModel]
    public class MessageController : ApiController
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public IEnumerable<MessageViewModel> Get()
        {
            var mostRecent = _messageRepository.FindMostRecent();
            var viewModels = mostRecent.Select(t => new MessageViewModel(t));
            return viewModels;
        }
    }
}
