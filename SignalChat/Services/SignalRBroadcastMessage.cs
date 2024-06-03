using Microsoft.AspNetCore.SignalR;
using SignalChat.Core.Contracts;
using SignalChat.Hubs;

namespace SignalChat.Services
{
    public class SignalRBroadcastMessage : IBroadcastMessage
    {
        private readonly IHubContext<NotificationHub> _hub;

        public SignalRBroadcastMessage(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public void BroadcastMessage(string message, string fromUsername)
        {
            _hub.Clients.All.SendAsync("broadcastMessage", message, fromUsername);
        }
    }
}
