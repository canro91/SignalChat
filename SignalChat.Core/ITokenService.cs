using Microsoft.IdentityModel.Tokens;

namespace SignalChat.Core
{
    public interface ITokenService
    {
        TokenValidationParameters TokenValidationParameters { get; }
        string CreateToken(string username);
    }
}
