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
using System.IO;

namespace TeamRoles.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> _userManager;

        public HomeController()
        {
            db = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        public ActionResult Index()
        {
            return View(db.Posts.AsEnumerable().Reverse().ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult JoinRequests()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List<GenericRequest> requests = user.Requests.ToList();
            List<RequestViewModel> viewmodelrequests = Convert(requests);
            return View(viewmodelrequests);
        }

        public ActionResult AcceptParentRequests()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List<GenericRequest> requests = user.Requests.ToList();
            List<RequestViewModel> viewmodelrequests = Convert(requests);
            return View(viewmodelrequests);
        }

        public ActionResult AdminRoleRequests()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List<GenericRequest> requests = user.Requests.ToList();
            List<RequestViewModel> viewmodelrequests = Convert(requests);
            return View(viewmodelrequests);
        }

        public ActionResult Requests()
        {
            if(User.IsInRole("Teacher"))
            {
                return RedirectToAction("JoinRequests");
            }
            else if(User.IsInRole("Student"))
            {
                return RedirectToAction("AcceptParentRequests");
            }
            else if(User.IsInRole("Admin"))
            {
                return RedirectToAction("AdminRoleRequests");
            }
            return View();
        }

        public ActionResult AcceptRequest(int? id)
        {
            GenericRequest req = db.Requests.Find(id);
            if(req.Type == "JoinCourse")
            {
                Course course = db.Courses.Find(req.Courseid);
                ApplicationUser student = db.Users.Find(req.User2id);
                course.ApplicationUsers.Add(student);
                db.Courses.Attach(course);

                Enrollment enrol = new Enrollment();
                enrol.Grade = -1;
                enrol.CourseId = course.CourseId;
                enrol.Course = course;
                enrol.UserId = student.Id;
                enrol.User = student;

                try
                {
                    db.Entry(course).State = EntityState.Modified;
                    db.Requests.Remove(req);
                    db.Enrollments.Add(enrol);
                    db.SaveChanges();
                }
                catch(Exception e)
                {
                    throw e;
                }
                return RedirectToAction("JoinRequests", "Home");
            }
            else if(req.Type ==  "ParentStudent")
            {
                ApplicationUser parent = db.Users.Find(req.User1id);
                ApplicationUser student = db.Users.Find(req.User2id);
                Child temp = new Child();
                temp.Childid = student.Id;
                temp.Parent.Add(parent);
                db.Children.Add(temp);
                db.Requests.Remove(req);
                db.SaveChanges();
                return RedirectToAction("AcceptParentRequests", "Home");
            }
            else
            {
                db.Requests.Remove(req);
                db.SaveChanges();
                return RedirectToAction("AdminRoleRequests", "Home");
            }
        }

        public ActionResult DeclineRequest(int? id)
        {
            GenericRequest req = db.Requests.Find(id);
            if (req.Type == "JoinCourse")
            {
                db.Requests.Remove(req);
                db.SaveChanges();
                return RedirectToAction("JoinRequests", "Home");
            }
            else if (req.Type == "ParentStudent")
            {
                db.Requests.Remove(req);
                db.SaveChanges();
                return RedirectToAction("AcceptParentRequests", "Home");
            }
            else
            {
                ApplicationUser deleted = db.Users.Find(req.User1id);
                db.Users.Remove(deleted);
                db.Requests.Remove(req);
                db.SaveChanges();
                return RedirectToAction("AdminRoleRequests", "Home");
            }
        }

        public List<RequestViewModel> Convert(List<GenericRequest> requests)
        {
            List<RequestViewModel> requestlist = new List<RequestViewModel>();
            foreach(var req in requests)
            {
                RequestViewModel temp = new RequestViewModel(req.ReqId,req.User1id,req.User2id,req.Courseid,req.Type,req.Role);
                requestlist.Add(temp);
            }
            return requestlist;
        }

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult Chat2()
        {
            return View(db.Users.ToList());
        }

    }
}