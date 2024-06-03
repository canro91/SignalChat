namespace SignalChat.Middlewares
{
    //public static class JwtSignalRExtension
    //{
    //    /// <summary>
    //    /// This enables JWT tokens to be used as authorization for SignalR
    //    /// To use this add the query parameter 'authtoken=[token]' when connection to a SignalR hub.
    //    /// </summary>
    //    /// <param name="app"></param>
    //    /// <param name="authQueryKey"></param>
    //    public static void UseJwtSignalRAuthentication(this IAppBuilder app, string authQueryKey)
    //    {
    //        app.Use<JwtSignalRMiddleware>(authQueryKey);
    //    }
    //}

    //public class JwtSignalRMiddleware : OwinMiddleware
    //{
    //    private readonly string AUTH_HEADER = "Authorization";

    //    public string AuthQueryKey { get; }

    //    public JwtSignalRMiddleware(OwinMiddleware next, string authQueryKey = "authtoken") : base(next)
    //    {
    //        AuthQueryKey = authQueryKey;
    //    }

    //    public override async Task Invoke(IOwinContext context)
    //    {
    //        if (string.IsNullOrWhiteSpace(context.Request.Headers[AUTH_HEADER]))
    //        {
    //            try
    //            {
    //                if (context.Request.QueryString.HasValue)
    //                {
    //                    var token = context.Request.QueryString.Value
    //                        .Split('&')
    //                        .SingleOrDefault(x => x.Contains(AuthQueryKey))?
    //                        .Split('=')
    //                        .Last();

    //                    if (!string.IsNullOrWhiteSpace(token))
    //                    {
    //                        context.Request.Headers.Add(AUTH_HEADER, new[] { $"Bearer {token}" });
    //                    }
    //                }
    //            }
    //            catch
    //            {
    //                // If it throws it doesn't matter, just continue without any changes
    //            }
    //        }
    //        await Next.Invoke(context);
    //    }
    //}
}