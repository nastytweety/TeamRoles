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
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> _userManager;

        public CoursesController()
        {
            db = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        // GET: Courses
        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List<Course> Courses = new List<Course>();
            Courses = user.Courses.ToList();
            return View(Courses);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Admin_Index()
        {
            return View(db.Courses.ToList());
        }

        public ActionResult Index_Selected()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            List<ApplicationUser> teachers = new List<ApplicationUser>();
            Index_SelectedViewModel model = new Index_SelectedViewModel();
            List<Course> Courses = new List<Course>();
            List<Course> ViewCourses = new List<Course>();
            Courses = user.Courses.ToList();
            

            foreach (var course in Courses)
            {
                List<ApplicationUser> alluser = course.ApplicationUsers.ToList();
                course.ApplicationUsers.Clear();
                foreach (var us in alluser)
                {
                    var isInRole = _userManager.IsInRole(us.Id, "Teacher");
                    if (isInRole)
                    {
                        course.ApplicationUsers.Add(us);
                        model.Courses.Add(course);
                    }
                }
            }
            return View(model);
        }

        public ActionResult Index_ToSelect(string id)
        {
            ApplicationUser teacher = db.Users.Find(id);
            ApplicationUser student = db.Users.Find(User.Identity.GetUserId());
            List<Course> TeacherCourses = teacher.Courses.ToList();
            List<Course> StudentCourses = student.Courses.ToList();
            List<Course> Courses = new List<Course>();
            bool found = false;
            foreach (var tcourse in TeacherCourses)
            {
                foreach (var scourse in StudentCourses)
                {
                    if (tcourse.CourseId == scourse.CourseId)
                    {
                        found = true;
                    }
                }
                if (found==false)
                {
                    Courses.Add(tcourse);
                }
                found = false;
            }
            return View(Courses);
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "CourseId,CourseName,CourseDescription,CoursePic")] Course course)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                List<Course> courses = user.Courses.ToList();
                foreach(var c in courses)
                {
                    if(c.CourseName == course.CourseName)
                    {
                        return RedirectToAction("Error");
                    }
                }
                course.ApplicationUsers.Add(db.Users.Find(User.Identity.GetUserId()));
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        public ActionResult Join(int id)
        {
            ApplicationUser student = db.Users.Find(User.Identity.GetUserId());
            Course course = db.Courses.Find(id);

            if (ModelState.IsValid)
            {
                course.ApplicationUsers = new List<ApplicationUser>();
                course.ApplicationUsers.Add(student);
                db.Courses.Attach(course);
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index_Selected", "Courses");
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,CourseName,CourseDescription,CoursePic")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult CourseHome(int? id)
        {
            Course course = db.Courses.Find(id);
            Index_SelectedViewModel model = new Index_SelectedViewModel();

            model.CourseName = course.CourseName;
            model.CoursePic = course.CoursePic;
            model.CourseDescription = course.CourseDescription;
            List<ApplicationUser> alluser = course.ApplicationUsers.ToList();
            foreach (var us in alluser)
            {
                Course list_course = new Course();
                    var isInStudentRole = _userManager.IsInRole(us.Id, "Student");
                    if (isInStudentRole)
                    {
                        list_course.ApplicationUsers.Add(us);
                        model.Courses.Add(list_course);
                    }
                    else
                    {
                        model.Teacher = us;
                    }
            }
            return View(model);
        }
    }
}
