using ServiceStack.Data;
using ServiceStack.OrmLite;
using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using SignalChat.Database.Entities;

namespace SignalChat.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _factory;

        public UserRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<User?> FindUserByUsernameAsync(string username)
        {
            using var db = _factory.OpenDbConnection();
            var result = (await db.SelectAsync<UserEntity>(x => x.Username == username))
                            .FirstOrDefault();

            return result == null
                    ? null
                    : new User
                    {
                        Username = result.Username!,
                        SaltedPassword = result.SaltedPassword!
                    };
        }

        public async Task SaveAsync(User newUser)
        {
            using var db = _factory.OpenDbConnection();

            var entity = new UserEntity
            {
                Username = newUser.Username,
                SaltedPassword = newUser.SaltedPassword
            };
            await db.InsertAsync(entity);
        }
    }
}