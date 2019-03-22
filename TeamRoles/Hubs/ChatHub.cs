using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace TeamRoles.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }

       /* public void Send(string senderName, string recipientName, string message)
        {
            Clients.User(recipientName).addNewMessageToPage(senderName, message);
        }*/
    }
}