using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using TeamRoles.Models;

namespace TeamRoles.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {

        private ApplicationDbContext db;
        private UserManager<ApplicationUser> _userManager;

        public StudentController()
        {
            db = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Student
        public ActionResult Index()
        {
            return View(FindStudent());
        }

        [Authorize(Roles = "Parent")]
        public ActionResult Parent_Index()
        {
            ApplicationUser parent = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List<Child> children = parent.Children.ToList();
            List<ApplicationUser> childrenlist = new List<ApplicationUser>();
            foreach(var child in children)
            {
                childrenlist.Add(db.Users.Find(child.Childid));
            }
            return View(childrenlist);
        }

        [Authorize(Roles = "Parent")]
        public ActionResult DeleteFromChild(string id)
        {
            ApplicationUser parent = db.Users.Find(User.Identity.GetUserId());
            Child child = db.Children.Where(ch=>ch.Childid == id).SingleOrDefault();
            parent.Children.Remove(child);
            db.Children.Remove(child);
            db.Entry(parent).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Parent_Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Admin_Index()
        {
            return View(FindStudent());
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCourse(string id, string coursename)
        {
            ApplicationUser student = db.Users.Find(id);
            Course course = db.Courses.FirstOrDefault(c => c.CourseName == coursename);
            student.Courses.Remove(course);
            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();
            
            return RedirectToAction("CourseHome","Courses", new { id = db.Courses.FirstOrDefault(c => c.CourseName == coursename).CourseId } );
        }

        public List<ApplicationUser> FindStudent()
        {
            List<ApplicationUser> Users = db.Users.ToList();
            List<ApplicationUser> usersInRole = new List<ApplicationUser>();

            foreach (var user in Users)
            {
                var isInRole = _userManager.IsInRole(user.Id, "Student");
                if (isInRole)
                {
                    usersInRole.Add(user);
                }
            }
            return usersInRole;
        }

        public ActionResult ParentConnect(string id)
        {
            ApplicationUser student = db.Users.Find(id);
            ApplicationUser parent = db.Users.Find(User.Identity.GetUserId());

            GenericRequest req = new GenericRequest();
            req.User1id = parent.Id;
            req.User2id = student.Id;
            req.Type = "ParentStudent";
            req.ApplicationUsers.Add(student);
            student.Requests.Add(req);
            db.Requests.Add(req);
            db.SaveChanges();
            return View("RequestSent");
        }
    }
}