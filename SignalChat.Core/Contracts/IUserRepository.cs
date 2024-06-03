using SignalChat.Core.Domain;

namespace SignalChat.Core.Contracts
{
    public interface IUserRepository
    {
        Task<User?> FindUserByUsernameAsync(string username);

        Task SaveAsync(User newUser);
    }
}