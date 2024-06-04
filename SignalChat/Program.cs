using FluentValidation;
using FluentValidation.AspNetCore;
using Foundatio.Queues;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using SignalChat.Bot;
using SignalChat.Bot.Contracts;
using SignalChat.Bot.Services;
using SignalChat.Configuration;
using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using SignalChat.Core.Tasks;
using SignalChat.Database.Migrations;
using SignalChat.Database.Repositories;
using SignalChat.Factories;
using SignalChat.Hubs;
using System.Text;

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

builder.Services
        .AddOptions<Login>()
        .Bind(builder.Configuration.GetSection("Login"))
        .ValidateDataAnnotations()
        .ValidateOnStart();

builder.Services.AddTransient<ITokenService>(provider =>
{
    var login = provider.GetRequiredService<IOptions<Login>>();
    return new TokenService(login.Value.Secret);
});
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IStockService, OnlineStockService>();
builder.Services.AddTransient<IBotService, BotService>();
builder.Services.AddSingleton<IQueue<Message>>(provider => SignalChatFactory.CreateQueue().Value);
builder.Services.AddTransient<ISendMessageService, SendMessageService>();

string connectionString = builder.Configuration.GetConnectionString("Database")!;
OrmLiteConfig.DialectProvider = SqlServerDialect.Provider;
builder.Services.AddSingleton<IDbConnectionFactory>(new OrmLiteConnectionFactory(connectionString));

var secret = builder.Configuration.GetValue<string>("Login:Secret");
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };
    });

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors(CorsPolicyName);
app.MapHub<NotificationHub>("/chatHub");

app.UseAuthentication();
app.UseAuthorization();
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