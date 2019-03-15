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
            return View(searching == null ? FindTeachers() : FindTeachers().Where(x => x.UserName.Contains(searching) || searching == null).ToList());
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Admin_Index()
        {
            return View(FindTeachers());
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

        public ApplicationUser FindTeacher(int CourseId)
        {
            Course course = db.Courses.Find(CourseId);
            List<ApplicationUser> Users =course.ApplicationUsers.ToList();
            

            foreach (var teacher in Users)
            {
                var isInRole = _userManager.IsInRole(teacher.Id, "Teacher");
                if (isInRole)
                {
                    return teacher;
                }
            }
            return null;
        }

        public List<ApplicationUser> FindTeachers()
        {
            List<ApplicationUser> Users = db.Users.ToList();
            List<ApplicationUser> usersInRole = new List<ApplicationUser>();

            foreach (var user in Users)
            {
                var isInRole = _userManager.IsInRole(user.Id, "Teacher");
                if (isInRole)
                {
                    usersInRole.Add(user);
                }
            }
            return (usersInRole);
        }

        public ActionResult Grades(string id, string coursename, string teacherid)
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
        public ActionResult Grades(SetGradeViewModel model)
        {
            ApplicationUser student = db.Users.Where(s => s.UserName == model.StudentName).SingleOrDefault();
            ApplicationUser teacher = db.Users.Where(s => s.UserName == model.TeacherName).SingleOrDefault();
            Course course = teacher.Courses.Where(c => c.CourseName == model.CourseName).SingleOrDefault();

            Enrollment enrol = new Enrollment();
            enrol.Grade = model.NumericGrade;
            enrol.CourseId = course.CourseId;
            enrol.Course = course;
            enrol.UserId = student.Id;
            enrol.User = student;

            if(!CheckIfGradeIsSet(enrol))
            {
                try
                {
                    db.Enrollments.Add(enrol);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return RedirectToAction("CourseHome", "Courses", new { id = course.CourseId });

        }

        public bool CheckIfGradeIsSet(Enrollment enrol)
        {
            List<Enrollment> listofenrol = db.Enrollments.Where(e=>e.CourseId == enrol.CourseId).ToList();

            foreach(var singleenrol in listofenrol)
            {
                if (singleenrol.User.Id.Equals(enrol.User.Id))
                {
                    try
                    {
                        singleenrol.Grade = enrol.Grade;
                        db.Entry(singleenrol).State = EntityState.Modified;
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    
                    }
                   
                }
            }
            return false;
        }

    }
}