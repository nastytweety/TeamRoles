using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TeamRoles.Models;

namespace TeamRoles.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext db;

        public RolesController()
        {
            db = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        //        // GET: Roles
        //        public ActionResult Index()
        //        {
        //            return View(db.Roles.ToList());
        //        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            var roles = db.Roles.ToList();
            foreach (var role in roles)
                if (role.Name.Equals(Role.Name) || Role.Name == null)
                {
                    TempData["sErrMsg"] = "This Role already exists";

                    return RedirectToAction("ManageUsers");
                }

            db.Roles.Add(Role);
            db.SaveChanges();
            return RedirectToAction("ManageUsers");
        }
        //public PartialViewResult ShowError(String sErrorMessage)
        //{
        //    return PartialView("_Create");
        //}

        public ActionResult Delete(string roleName)
        {
            var list = db.Roles.OrderBy(r => r.Name).ToList()
                .Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            var thisRole =
                db.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));

            //if (thisRole == null || list == null)
            //{
            //    return View();
            //}

            db.Roles.Remove(thisRole);
            db.SaveChanges();

            //ViewBag.SuccessMessage = "Success!";

            return RedirectToAction("ManageUsers");
        }


        //        // GET: /Roles/Edit/5
        //        public ActionResult Edit(string roleName)
        //        {
        //            var thisRole = db.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        //
        //            return View(thisRole);
        //        }

        /*//
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }*/


        public ActionResult ManageUsers()
        {
            // prepopulat roles for the view dropdown
            var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            var user = db.Users.FirstOrDefault(u =>
                u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));

            var list = db.Roles.OrderBy(r => r.Name).ToList()
                .Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            if (user == null || RoleName == null)
            {
                ViewBag.Message = "Role cannot created !";
                return View("ManageUsers");
            }
            else if (manager.IsInRole(user.Id, RoleName))
            {
                ViewBag.RoleMessage = "User has already this role";
                return View("ManageUsers");
            }

            manager.AddToRole(user.Id, RoleName);

            return View("ManageUsers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));

            var list = db.Roles.OrderBy(r => r.Name)
                    .ToList()
                    .Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name })
                    .ToList();
            ViewBag.Roles = list;

            if (user == null || list == null)
            {
                ViewBag.ResultMessage = "Cannot view Roles !";
            }

            else if (!string.IsNullOrWhiteSpace(UserName))
            {
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                ViewBag.RolesForThisUser = manager.GetRoles(user.Id);
            }

            return View("ManageUsers");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            var user = db.Users.FirstOrDefault(u =>
                u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));

            if (user == null || RoleName == null)
            {
                ViewBag.Message1 = "Wrong check again !";
            }
            else if (manager.IsInRole(user.Id, RoleName))
            {
                manager.RemoveFromRole(user.Id, RoleName);
                ViewBag.Message2 = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.Message3 = "This user doesn't belong to selected role.";
            }
            // prepopulat roles for the view dropdown
            var list = db.Roles.OrderBy(r => r.Name).ToList()
                .Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUsers");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}