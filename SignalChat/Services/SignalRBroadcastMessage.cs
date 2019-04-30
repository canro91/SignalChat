using Microsoft.AspNet.SignalR;
using SignalChat.Core.Contracts;
using SignalChat.Hubs;

namespace SignalChat.Services
{
    public class SignalRBroadcastMessage : IBroadcastMessage
    {
        public void BroadcastMessage(string message, string fromUsername)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            hub.Clients.All.broadcastMessage(message, fromUsername);
        }
    }
}
