using FluentValidation;
using FluentValidation.AspNetCore;
using Foundatio.Queues;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using SignalChat.Bot;
using SignalChat.Bot.Contracts;
using SignalChat.Bot.Services;
using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using SignalChat.Core.Tasks;
using SignalChat.Database.Migrations;
using SignalChat.Database.Repositories;
using SignalChat.Factories;
using SignalChat.Hubs;

const string CorsPolicyName = "ApiCorsPolicy";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});
builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IProtectPasswordService, ProtectPasswordService>();
builder.Services.AddTransient<IRegisterService, RegisterService>();
builder.Services.AddTransient<ITokenService>(provider =>
{
    //TODO
    var key = "ASuperSecretSecret";
    return new TokenService(key);
});
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IStockService, OnlineStockService>();
builder.Services.AddTransient<IBotService, BotService>();
builder.Services.AddSingleton<IQueue<Message>>(provider => SignalChatFactory.CreateQueue().Value);
builder.Services.AddTransient<ISendMessageService, SendMessageService>();

string connectionString = builder.Configuration.GetConnectionString("Database")!;
OrmLiteConfig.DialectProvider = SqlServerDialect.Provider;
builder.Services.AddSingleton<IDbConnectionFactory>(new OrmLiteConnectionFactory(connectionString));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors(CorsPolicyName);
app.MapHub<NotificationHub>("/chatHub");
app.MapControllers();

var isDevelopment = builder.Environment.IsDevelopment();
if (isDevelopment)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var factory = services.GetRequiredService<IDbConnectionFactory>();
    var migrator = new Migrator(factory, typeof(Migration001).Assembly);
    migrator.Run();
}

app.Run();

public partial class Program
{
}