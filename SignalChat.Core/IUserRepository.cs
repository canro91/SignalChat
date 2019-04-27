namespace SignalChat.Core
{
    public interface IUserRepository
    {
        User FindUserByUsername(string username);
        void Save(User newUser);
    }
}
