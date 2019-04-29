using SignalChat.Core.Contracts;
using SignalChat.Core.Insfrastructure;
using SignalChat.Filters;
using SignalChat.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace SignalChat.Controllers
{
    [Authorize]
    [ValidateModel]
    public class MessageController : ApiController
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            _messageRepository = new MessageRepository(connectionString);
        }

        public IEnumerable<MessageViewModel> Get()
        {
            var mostRecent = _messageRepository.FindMostRecent();
            var viewModels = mostRecent.Select(t => new MessageViewModel(t));
            return viewModels;
        }
    }
}
