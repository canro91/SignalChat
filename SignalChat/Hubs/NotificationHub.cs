using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalChat.Hubs
{
    public class NotificationHub : Hub
    {
        public override Task OnConnected()
        {
            // 1. Add to group
            if (Context.User?.Identity?.IsAuthenticated == false)
                throw new System.Exception("Usted no pasara mas alla de este nivel");

            // 2. Add to connected
            var claims = (Context.User.Identity as ClaimsIdentity).Claims;
            var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

            // 3. Update user count
            Clients.All.updateUsers(username);

            // 4. Welcome message
            Clients.Client(Context.ConnectionId).broadcastMessage(
                "Welcome to the chat room, " + username, "[server]");

            return base.OnConnected();
        }

        [Authorize]
        public void Send(string message)
        {
            var claims = (Context.User.Identity as ClaimsIdentity).Claims;
            var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

            Clients.All.broadcastMessage(message, username);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // 1. Remove from group
            //Groups.Remove(Context.ConnectionId, "authenticated");

            // 2. Remove from connected
            //var disconnected = ConnectedClients.FirstOrDefault(t => t.ID == Context.ConnectionId);
            //ConnectedClients.Remove(disconnected);

            // 3. Update user count
            //var count = ConnectedClients.Count;
            //var allUsernames = ConnectedClients.Select(t => t.Username).ToList();
            //Clients.All.updateUsers(count, allUsernames);

            return base.OnDisconnected(stopCalled);
        }
    }

    public class Client
    {
        public string Username { get; set; }
        public string ID { get; set; }
    }
}