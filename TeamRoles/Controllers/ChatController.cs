using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamRoles.Models;

namespace TeamRoles.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Chat
        public ActionResult Index()
        {
            // ChatViewModel vm = new ChatViewModel()
            // {
            List<ApplicationUser> Users = db.Users.ToList();
           // };
            return View(Users);
        }
    }
}