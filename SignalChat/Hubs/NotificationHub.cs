using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalChat.Core.Contracts;

namespace SignalChat.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ISendMessageService _messageService;

        public NotificationHub(ISendMessageService messageService)
        {
            _messageService = messageService;
        }

        public async override Task OnConnectedAsync()
        {
            var username = Context.UserIdentifier
                                ?? throw new InvalidOperationException(SN.UnauthenticatedUser);

            await Clients.Others.SendAsync("updateUsers", username);

            var welcome = string.Format(SN.WelcomeMessage, username);
            var server = SN.ServerUsername;
            await Clients.Client(Context.ConnectionId)
                        .SendAsync("broadcastMessage", welcome, server);

            await base.OnConnectedAsync();
        }

        public async Task Send(string message)
        {
            var username = Context.UserIdentifier!;

            var handledAsCommand = await _messageService.SendAsync(username, message);
            if (!handledAsCommand)
            {
                await Clients.All.SendAsync("broadcastMessage", message, username);
            }
        }
    }
}