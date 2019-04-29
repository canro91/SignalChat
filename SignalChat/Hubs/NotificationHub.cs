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
            if (Context.User?.Identity?.IsAuthenticated == false)
                throw new System.Exception(SN.UnauthenticatedUser);

            var claims = (Context.User.Identity as ClaimsIdentity).Claims;
            var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

            Clients.All.updateUsers(username);

            var welcome = string.Format(SN.WelcomeMessage, username);
            var server = SN.ServerUsername;
            Clients.Client(Context.ConnectionId).broadcastMessage(welcome, server);

            return base.OnConnected();
        }

        [Authorize]
        public void Send(string message)
        {
            var claims = (Context.User.Identity as ClaimsIdentity).Claims;
            var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

            Clients.All.broadcastMessage(message, username);
        }
    }
}