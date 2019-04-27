namespace SignalChat.Core
{
    public interface IProtectPasswordService
    {
        string ProtectPassword(string plainTextPassword);
        bool VerifyPassword(string plainTextPassword, string saltedPassword);
    }
}
