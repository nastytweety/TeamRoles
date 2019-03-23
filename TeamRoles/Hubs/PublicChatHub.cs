using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using Microsoft.AspNet.SignalR;
using TeamRoles.Controllers;
//using Project2.Repositories;


namespace TeamRoles.Hubs
{
    [Authorize]
    public class PublicChatHub : Hub
    {
        private static readonly ConnectionMapping<string> PublicConnections = new ConnectionMapping<string>();

        private MessageRepository messageDb = new MessageRepository();
        private UserRepository userDb = new UserRepository();

        public async Task Broadcast(string message) // who == recipient
        {
            string caller_username = Context.User.Identity.Name;

            var user = await userDb.GetUserByUsername(caller_username);

            // in order to show the timestamp of the message

            DateTime message_date = DateTime.UtcNow;

            // send message to Database
            var messageId = await messageDb.SendMessage(caller_username, "ChatBot", message); // allaksame tin send message tou repo

            //send message to all connection ids of the caller
            foreach (var connectionId in PublicConnections.GetConnections(caller_username))
            {
                Clients.Client(connectionId).receive("me", message, message_date, user.ProfilePic, messageId);
            }

            // send message to other clients
            Clients.AllExcept(PublicConnections.GetConnections(caller_username).ToArray()).receive(caller_username, message, message_date, user.ProfilePic, messageId);
        }


        //====================================================================================================================//

        public void ClickHeart(string heart_id, int heart_clicks)
        {
            Clients.Others.getHearts(heart_id, heart_clicks);
        }

        //====================================================================================================================//

        public void DeleteAll()
        {
            Clients.All.delete();
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            PublicConnections.Add(name, Context.ConnectionId);
            Clients.All.updateCount(PublicConnections.Count); // returns number of connected clients
            Clients.All.getList(PublicConnections.GetUsernames());


            Clients.Client(Context.ConnectionId).getUsername(name);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            PublicConnections.Remove(name, Context.ConnectionId);
            Clients.All.updateCount(PublicConnections.Count); // returns number of connected clients
            Clients.All.getList(PublicConnections.GetUsernames());

            //List<string> x = new List<string>();
            //x = PublicConnections.GetUsernames();
            //Clients.All.getList(x);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!PublicConnections.GetConnections(name).Contains(Context.ConnectionId))
            {
                PublicConnections.Add(name, Context.ConnectionId);
            }
            Clients.All.updateCount(PublicConnections.Count); // returns number of connected clients
            Clients.All.getList(PublicConnections.GetUsernames());

            return base.OnReconnected();
        }
    }
}