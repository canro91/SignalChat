using Microsoft.Owin;
using System.Linq;
using System.Threading.Tasks;

namespace SignalChat.SignalR.Jwt
{
    public class JwtSignalRMiddleware : OwinMiddleware
    {
        private readonly string AUTH_HEADER = "Authorization";

        public string AuthQueryKey { get; }

        public JwtSignalRMiddleware(OwinMiddleware next, string authQueryKey = "authtoken") : base(next)
        {
            AuthQueryKey = authQueryKey;
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (string.IsNullOrWhiteSpace(context.Request.Headers[AUTH_HEADER]))
            {
                try
                {
                    if (context.Request.QueryString.HasValue)
                    {
                        var token = context.Request.QueryString.Value
                            .Split('&')
                            .SingleOrDefault(x => x.Contains(AuthQueryKey))?
                            .Split('=')
                            .Last();

                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            context.Request.Headers.Add(AUTH_HEADER, new[] { $"Bearer {token}" });
                        }
                    }
                }
                catch
                {
                    // If it throws it doesn't matter, just continue without any changes
                }
            }
            await Next.Invoke(context);
        }
    }
}
