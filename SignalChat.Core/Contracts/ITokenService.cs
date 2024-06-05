using Microsoft.IdentityModel.Tokens;

namespace SignalChat.Core.Contracts;

public interface ITokenService
{
    TokenValidationParameters TokenValidationParameters { get; }

    string CreateToken(string username);
}