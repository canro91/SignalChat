using ServiceStack.Data;
using ServiceStack.OrmLite;
using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using SignalChat.Database.Entities;

namespace SignalChat.Database.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly IDbConnectionFactory _factory;

    public MessageRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<Message>> FindMostRecentAsync(int count = 50)
    {
        using var db = _factory.OpenDbConnection();
        var query = db.From<MessageEntity>()
                        .OrderBy(x => x.DeliveredAt)
                        .Take(count);
        var result = await db.SelectAsync(query);

        return result.Select(x => new Message
        {
            Username = x.Username,
            Body = x.Body,
            DeliveredAt = x.DeliveredAt
        });
    }

    public async Task SaveAsync(Message message)
    {
        using var db = _factory.OpenDbConnection();

        var entity = new MessageEntity
        {
            Username = message.Username,
            Body = message.Body,
            DeliveredAt = message.DeliveredAt
        };
        await db.InsertAsync(entity);
    }
}