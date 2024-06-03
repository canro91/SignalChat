using Microsoft.AspNetCore.SignalR;
using SignalChat.Core.Contracts;

namespace SignalChat.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ISendMessageService _messageService;

        public NotificationHub(ISendMessageService messageService)
        {
            _messageService = messageService;
        }

        public async override Task OnConnectedAsync()
        {
            //if (Context.User?.Identity?.IsAuthenticated == false)
            //    throw new System.Exception(SN.UnauthenticatedUser);

            await Clients.All.SendAsync("updateUsers", CurrentUsername);

            //var welcome = string.Format(SN.WelcomeMessage, CurrentUsername);
            var welcome = $"Welcome to the chat, {CurrentUsername}!";
            //var server = SN.ServerUsername;
            var server = "SignalChat";
            await Clients.Client(Context.ConnectionId).SendAsync("broadcastMessage", welcome, server);

            await base.OnConnectedAsync();
        }

        // TODO
        //[Authorize]
        public async Task SendMessage(string user, string message)
        {
            // TODO
            //var handleAsCommand = _messageServices.Send(CurrentUsername, message);
            //if (!handleAsCommand)
            //{
            //    Clients.All.broadcastMessage(message, CurrentUsername);
            //}

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        private string CurrentUsername
        {
            get
            {
                // TODO
                //var claims = (Context.User.Identity as ClaimsIdentity).Claims;
                //var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                //return username;
                return Context.ConnectionId;
            }
        }
    }
}