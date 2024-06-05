using Foundatio.Extensions.Hosting.Jobs;
using Foundatio.Queues;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using SignalChat.Bot.Contracts;
using SignalChat.Bot.Services;
using SignalChat.Configuration;
using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using SignalChat.Core.Tasks;
using SignalChat.Database.Repositories;
using SignalChat.Hubs;
using SignalChat.Jobs;
using SignalChat.Services;

namespace SignalChat.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration loginSection)
    {
        services.AddTransient<IProtectPasswordService, ProtectPasswordService>();
        services.AddTransient<IRegisterService, RegisterService>();

        services
            .AddOptions<Login>()
            .Bind(loginSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddTransient<ITokenService>(provider =>
        {
            var login = provider.GetRequiredService<IOptions<Login>>();
            return new TokenService(login.Value.Secret);
        });
        services.AddTransient<ILoginService, LoginService>();
        services.AddTransient<ISendMessageService, SendMessageService>();

        services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
        services.AddTransient<IBroadcastMessage, SignalRBroadcastMessage>();
    }

    public static void AddBotServices(this IServiceCollection services)
    {
        services.AddTransient<IBotService, BotService>();
        services.AddTransient<IStockService, OnlineStockService>();
        services.AddSingleton<IQueue<Message>>(provider => new InMemoryQueue<Message>());
        services.AddJob<SendMessageIntoChatRoom>();
    }

    public static void AddDataServices(this IServiceCollection services, string connectionString)
    {
        services.AddTransient<IMessageRepository, MessageRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        OrmLiteConfig.DialectProvider = SqlServerDialect.Provider;
        services.AddSingleton<IDbConnectionFactory>(new OrmLiteConnectionFactory(connectionString));
    }
}