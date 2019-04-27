namespace SignalChat.Core
{
    public interface IRegisterService
    {
        void RegisterUser(string username, string plainTextPassword);
    }
}
