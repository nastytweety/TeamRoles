using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeamRoles.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;


namespace TeamRoles.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> _userManager;

        public HomeController()
        {
            db = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List<Post> Posts = new List<Post>();
            if (User.IsInRole("Teacher"))
            {
                Posts = db.Posts.Where(p=>p.UserRole == "Teacher").ToList();
            }
            else if (User.IsInRole("Student"))
            {
                Posts = user.Posts.Where(p => p.UserRole == "Student").ToList();
            }
            else if (User.IsInRole("Admin"))
            {
                Posts = db.Posts.ToList();
            }
            ViewBag.Posts = Posts.AsEnumerable().Reverse();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}