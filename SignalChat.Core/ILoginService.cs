namespace SignalChat.Core
{
    public interface ILoginService
    {
        string Login(string username, string plainTextPassword);
    }
}
