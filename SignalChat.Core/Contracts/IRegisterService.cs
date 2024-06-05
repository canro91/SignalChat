namespace SignalChat.Core.Contracts;

public interface IRegisterService
{
    Task RegisterUserAsync(string username, string plainTextPassword);
}
