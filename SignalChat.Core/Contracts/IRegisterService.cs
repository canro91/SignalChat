namespace SignalChat.Core.Contracts
{
    public interface IRegisterService
    {
        void RegisterUser(string username, string plainTextPassword);
    }
}
