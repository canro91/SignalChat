namespace SignalChat.Core.Contracts;

public interface IProtectPasswordService
{
    string ProtectPassword(string plainTextPassword);

    bool VerifyPassword(string plainTextPassword, string saltedPassword);
}