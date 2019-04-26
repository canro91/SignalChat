using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using SignalChat.SignalR.Jwt;
using System.Text;

[assembly: OwinStartup(typeof(SignalChat.Startup))]
namespace SignalChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseJwtSignalRAuthentication(authQueryKey: "token");

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-256-bit-secret")),
                    ValidateLifetime = false,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }
            });

            app.MapSignalR();
        }
    }
}
