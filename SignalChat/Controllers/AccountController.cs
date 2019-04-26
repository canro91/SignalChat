using Microsoft.IdentityModel.Tokens;
using SignalChat.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace SignalChat.Controllers
{
    [Authorize]
    public class AccountController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Login([FromBody] LoginViewModel loginViewModel)
        {
            // TODO Token generator
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, loginViewModel.Username));

            var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-256-bit-secret")), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(issuer: null, audience: null, subject: claimsIdentity,
                                            notBefore: null, expires: null, issuedAt: null,
                                            signingCredentials: credentials);

            return ReplyWith(token.RawData);
        }

        private IHttpActionResult ReplyWith(string token)
        {
            if (token == null)
            {
                return Unauthorized();
            }

            //if (!string.IsNullOrEmpty(result.Error))
            //{
            //    return Unauthorized(new AuthenticationHeaderValue(JwtController.AuthScheme, result.Error));
            //}

            return Ok(token);
        }
    }
}
