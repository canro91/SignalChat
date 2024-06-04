using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using SignalChat.Database.Migrations;
using SignalChat.Extensions;
using SignalChat.Hubs;
using System.Text;

const string CorsPolicyName = "ApiCorsPolicy";
const string ChatHubName = "/chatHub";

var (builder, services, config) = WebApplication.CreateBuilder(args);

services.AddSignalR();

services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});
services.AddControllers();

services.AddFluentValidationAutoValidation();
services.AddValidatorsFromAssemblyContaining<Program>();

var connectionString = config.GetConnectionString("Database")!;
services.AddDataServices(connectionString);

var login = config.GetRequiredSection("Login");
services.AddServices(login);

services.AddBotServices();

services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var secret = login.GetValue<string>("Secret");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var jwtToken = context.Request.Query["token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(jwtToken)
                    && path.StartsWithSegments(ChatHubName))
                {
                    context.Token = jwtToken;
                }

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors(CorsPolicyName);
app.MapHub<NotificationHub>(ChatHubName);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var isDevelopment = builder.Environment.IsDevelopment();
if (isDevelopment)
{
    using var scope = app.Services.CreateScope();
    var provider = scope.ServiceProvider;

    var factory = provider.GetRequiredService<IDbConnectionFactory>();
    var migrator = new Migrator(factory, typeof(Migration001).Assembly);
    migrator.Run();
}

app.Run();

public partial class Program
{
}