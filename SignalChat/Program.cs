using FluentValidation;
using FluentValidation.AspNetCore;
using Foundatio.Queues;
using SignalChat.Bot;
using SignalChat.Bot.Contracts;
using SignalChat.Bot.Services;
using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using SignalChat.Core.Insfrastructure;
using SignalChat.Core.Tasks;
using SignalChat.Factories;
using SignalChat.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddTransient<IMessageRepository>(provider =>
{
    return new MessageRepository();
});
builder.Services.AddTransient<IUserRepository>(provider =>
{
    return new UserRepository();
});
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


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<NotificationHub>("/chatHub");
app.Run();

public partial class Program
{
}
