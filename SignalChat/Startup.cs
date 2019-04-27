﻿using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using SignalChat.Core;
using SignalChat.SignalR.Jwt;
using System.Configuration;

[assembly: OwinStartup(typeof(SignalChat.Startup))]
namespace SignalChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseJwtSignalRAuthentication(authQueryKey: "token");

            var secret = ConfigurationManager.AppSettings["Secret"];
            var tokenService = new TokenService(secret);

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                TokenValidationParameters = tokenService.TokenValidationParameters
            });

            app.MapSignalR();
        }
    }
}
