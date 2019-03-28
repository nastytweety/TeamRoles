using Microsoft.AspNet.SignalR;
using TeamRoles.Controllers;
using TeamRoles.Models;
using System;
//using TeamRoles.Repositories;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;


namespace TeamRoles.Hubs
{
    [Authorize]
    public class PrivateChatHub : Hub
    {
        public static readonly ConnectionMapping<string> Connections = new ConnectionMapping<string>();
        private MessageRepository msgRepo = new MessageRepository();
        private UserRepository userRepo = new UserRepository();

        public void ReturnConnectectedUsers()
        {
            var connectedUsers = Connections;
            Clients.All.send(connectedUsers);
        }

        public async Task Send(string who, string message)
        {
            var success = await msgRepo.SendMessage(Context.User.Identity.Name, who, message);

            DateTime date = DateTime.UtcNow;
            var user = await userRepo.GetUserByUsername(Context.User.Identity.Name);


            foreach (var connectionId in Connections.GetConnections(who))
            {
                //Clients.Caller.SendMessagetoSelf("Me", message);
                await Clients.Client(connectionId).AddNewMessageToPage(user.UserName, "/Users/"+user.UserName+"/"+user.ProfilePic, message, date);
            }

            await Clients.Caller.addNewMessageToMyPage("/Users/" + user.UserName + "/" + user.ProfilePic, message, date);
        }

        // when a user is connected with the hub, adds to the dictionary his name and his signalR connection Id
        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            Connections.Add(name, Context.ConnectionId);
            Clients.All.sendConnectedUsers(Json.Encode(Connections._connections));


            return base.OnConnected();
        }

        // when a user is disconnected from the hub, removes from the dictionary his name and his singlaR connection Id
        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            Connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        // when a user is reconnected to the hub, if the dictionary lost his details, adds it again.
        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!Connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                Connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}