using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;

namespace SignalChat.Core.Insfrastructure
{
    public class UserRepository : IUserRepository
    {
        public User FindUserByUsername(string username)
        {
            //TODO
            //var connection = new SqlConnection(_connectionString);
            //var user = connection.Single<User>("sp_Users_FindUserByUsername", new { Username = username });
            //return user;
            return new User
            {
                ID = Guid.NewGuid(),
                Username = "lorem ipsum",
                SaltedPassword = "lorem ipsum"
            };
        }

        public void Save(User newUser)
        {
            //var connection = new SqlConnection(_connectionString);
            //connection.Execute("sp_Users_Save", newUser);
        }
    }
}
