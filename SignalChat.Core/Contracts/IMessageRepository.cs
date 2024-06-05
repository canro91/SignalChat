using SignalChat.Core.Domain;

namespace SignalChat.Core.Contracts;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> FindMostRecentAsync(int count = 50);

    Task SaveAsync(Message message);
}