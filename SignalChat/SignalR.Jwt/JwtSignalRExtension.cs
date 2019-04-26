using Owin;

namespace SignalChat.SignalR.Jwt
{
    public static class JwtSignalRExtension
    {
        /// <summary>
        /// This enables JWT tokens to be used as authorization for SignalR
        /// To use this add the query parameter 'authtoken=[token]' when connection to a SignalR hub.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="authQueryKey"></param>
        public static void UseJwtSignalRAuthentication(this IAppBuilder app, string authQueryKey)
        {
            app.Use<JwtSignalRMiddleware>(authQueryKey);
        }
    }
}