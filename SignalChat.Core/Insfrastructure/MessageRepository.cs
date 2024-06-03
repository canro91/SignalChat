using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;

namespace SignalChat.Core.Insfrastructure
{
    public class MessageRepository : IMessageRepository
    {
        public IEnumerable<Message> FindMostRecent(int count = 50)
        {
            // TODO
            //var connection = new SqlConnection(_connectionString);
            //var messages = connection.Query<Message>("sp_Messages_FindMostRecent", new { Count = count });
            //return messages;
            return Enumerable.Empty<Message>();
        }

        public void Save(Message message)
        {
            // TODO
            //var connection = new SqlConnection(_connectionString);
            //connection.Execute("sp_Messages_Save", message);
        }
    }
}
