using SignalChat.Core.Domain;

namespace SignalChat.Core.Contracts
{
    public interface IMessageRepository
    {
        IEnumerable<Message> FindMostRecent(int count = 50);

        void Save(Message message);
    }
}