using Owin;
using SignalChat.Core.Contracts;
using SignalChat.Core.Insfrastructure;
using SignalChat.Core.Tasks;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Configuration;

namespace SignalChat.Middlewares
{
    public static class SimpleInjectorMiddlewareExtensions
    {
        public static void MapDependencies(this IAppBuilder app, Container container)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.Register<IMessageRepository>(() =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                return new MessageRepository(connectionString);
            });
            container.Register<IUserRepository>(() =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                return new UserRepository(connectionString);
            });
            container.Register<IProtectPasswordService, ProtectPasswordService>();
            container.Register<IRegisterService, RegisterService>();
            container.Register<ITokenService>(() =>
            {
                var key = ConfigurationManager.AppSettings["Secret"];
                return new TokenService(key);
            });
            container.Register<ILoginService, LoginService>();
            container.Verify();

            app.Use(async (context, next) =>
            {
                using (AsyncScopedLifestyle.BeginScope(container))
                {
                    await next();
                }
            });
        }
    }
}
