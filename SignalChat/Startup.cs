using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using SignalChat.Core.Contracts;
using SignalChat.HubActivator;
using SignalChat.Middlewares;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Web.Http;

[assembly: OwinStartup(typeof(SignalChat.Startup))]
namespace SignalChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseJwtSignalRAuthentication(authQueryKey: "token");

            var container = new Container();
            app.MapDependencies(container);

            ITokenService tokenService = container.GetInstance<ITokenService>();
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                TokenValidationParameters = tokenService.TokenValidationParameters
            });

            var config = new HttpConfiguration
            {
                DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container)
            };
            WebApiConfig.Register(config);
            app.UseWebApi(config);

            var activator = new SimpleInjectorHubActivator(container);
            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => activator);

            app.MapSignalR();
        }
    }
}
