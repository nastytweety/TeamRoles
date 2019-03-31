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
using TeamRoles.Models;
using TeamRoles.Repositories;


namespace TeamRoles.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> _userManager;

        public TeacherController()
        {
            db = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Teacher
        public ActionResult Index(string searching)
        {
            UserRepository repository = new UserRepository();
            return View(searching == null ? repository.FindTeachers() : repository.FindTeachers().Where(x => x.UserName.Contains(searching) || searching == null).ToList());
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Admin_Index()
        {
            UserRepository repository = new UserRepository();
            return View(repository.FindTeachers());
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
            ApplicationUser teacher = db.Users.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            UserRepository repository = new UserRepository();
            repository.DeleteTeacher(teacher);
            return RedirectToAction("Admin_Index");
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult SetGrades(string id, string coursename, string teacherid)
        {
            ApplicationUser student = db.Users.Find(id);
            ApplicationUser teacher = db.Users.Find(teacherid);
            SetGradeViewModel model = new SetGradeViewModel();
            model.CourseName = coursename;
            model.StudentName = student.UserName;
            model.TeacherName = teacher.UserName;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult SetGrades(SetGradeViewModel model)
        {
            ApplicationUser student = db.Users.Where(s => s.UserName == model.StudentName).SingleOrDefault();
            ApplicationUser teacher = db.Users.Where(s => s.UserName == model.TeacherName).SingleOrDefault();
            Course course = teacher.Courses.Where(c => c.CourseName == model.CourseName).SingleOrDefault();
            Enrollment enrol = course.Enrollments.Where(u=>u.UserId == student.Id).SingleOrDefault();

            if(enrol!=null)
            {
                try
                {
                    enrol.Grade = model.NumericGrade;
                    db.Entry(enrol).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return RedirectToAction("CourseHome", "Courses", new { id = course.CourseId });
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