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
            return View(usersInRole);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Admin_Index()
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
            return View(usersInRole);
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

        public ActionResult DeleteFromCourse(string id, string coursename)
        {
            ApplicationUser student = db.Users.Find(id);
            Course course = db.Courses.FirstOrDefault(c => c.CourseName == coursename);
            student.Courses.Remove(course);
            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();
            
            return RedirectToAction("CourseHome","Courses", new { id = db.Courses.FirstOrDefault(c => c.CourseName == coursename).CourseId } );
        }
    }
}