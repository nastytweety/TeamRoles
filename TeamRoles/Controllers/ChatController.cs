using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamRoles.Models;
using TeamRoles.Hubs;
using System.Threading.Tasks;

namespace TeamRoles.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private MessageRepository msgRepo = new MessageRepository();
        private UserRepository userRepo = new UserRepository();
        
        // GET: Chat
        public async Task<ActionResult> Index()
        {

            List<ApplicationUser> Users = new List<ApplicationUser>();

            var connectedUsers = PrivateChatHub.Connections.GetUsernames();
            foreach(var user in connectedUsers)
            {
                Users.Add(await userRepo.GetUserByUsername(user));
            }
           return View(Users);
        }
    }
}