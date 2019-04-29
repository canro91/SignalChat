using SignalChat.Core.Domain;

namespace SignalChat.Core.Contracts
{
    public interface IUserRepository
    {
        User FindUserByUsername(string username);
        void Save(User newUser);
    }
}
