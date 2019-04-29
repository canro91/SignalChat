using Insight.Database;
using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SignalChat.Core.Insfrastructure
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string _connectionString;

        public MessageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        static MessageRepository()
        {
            Insight.Database.SqlInsightDbProvider.RegisterProvider();
        }

        public IEnumerable<Message> FindMostRecent(int count = 50)
        {
            var connection = new SqlConnection(_connectionString);
            var messages = connection.Query<Message>("sp_Messages_FindMostRecent", new { Count = count });
            return messages;
        }
    }
}
