using Insight.Database;
using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using System.Data.SqlClient;

namespace SignalChat.Core.Insfrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        static UserRepository()
        {
            Insight.Database.SqlInsightDbProvider.RegisterProvider();
        }

        public User FindUserByUsername(string username)
        {
            var connection = new SqlConnection(_connectionString);
            var user = connection.Single<User>("sp_Users_FindUserByUsername", new { Username = username });
            return user;
        }

        public void Save(User newUser)
        {
            var connection = new SqlConnection(_connectionString);
            connection.Execute("sp_Users_Save", newUser);
        }
    }
}
