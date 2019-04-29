using Microsoft.IdentityModel.Tokens;
using SignalChat.Core.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SignalChat.Core.Tasks
{
    public class TokenService : ITokenService
    {
        private readonly string _secret;

        public TokenService(string secret)
        {
            _secret = secret;
        }

        public TokenValidationParameters TokenValidationParameters
        {
            get
            {
                return new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)),
                    ValidateLifetime = false,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            }
        }

        public string CreateToken(string username)
        {
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, username));

            var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(issuer: null, audience: null, subject: claimsIdentity,
                                            notBefore: null, expires: null, issuedAt: null,
                                            signingCredentials: credentials);

            return token.RawData;
        }
    }
}
