using SignalChat.Core.Domain;
using System.Collections.Generic;

namespace SignalChat.Core.Contracts
{
    public interface IMessageRepository
    {
        IEnumerable<Message> FindMostRecent(int count = 50);
    }
}
