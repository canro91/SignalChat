﻿using Microsoft.AspNet.SignalR;
using SignalChat.Core.Contracts;
using SignalChat.Core.Insfrastructure;
using SignalChat.Core.Tasks;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalChat.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ISendMessageService _messageService;

        public NotificationHub()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            var messageRepository = new MessageRepository(connectionString);

            _messageService = new SendMessageService(messageRepository);
        }

        public override Task OnConnected()
        {
            if (Context.User?.Identity?.IsAuthenticated == false)
                throw new System.Exception(SN.UnauthenticatedUser);

            Clients.All.updateUsers(CurrentUsername);

            var welcome = string.Format(SN.WelcomeMessage, CurrentUsername);
            var server = SN.ServerUsername;
            Clients.Client(Context.ConnectionId).broadcastMessage(welcome, server);

            return base.OnConnected();
        }

        [Authorize]
        public void Send(string message)
        {
            _messageService.Send(CurrentUsername, message);

            Clients.All.broadcastMessage(message, CurrentUsername);
        }

        private string CurrentUsername
        {
            get
            {
                var claims = (Context.User.Identity as ClaimsIdentity).Claims;
                var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                return username;
            }
        }
    }
}