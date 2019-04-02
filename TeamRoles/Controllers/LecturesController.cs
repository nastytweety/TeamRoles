using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Validation;
using TeamRoles.Models;
using TeamRoles.Repositories;

namespace TeamRoles.Controllers
{
    public class LecturesController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> _userManager;

        public LecturesController()
        {
            db = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(Lecture lecture,int CourseId)
        {
            CoursesRepository repository = new CoursesRepository();
            if (!repository.CheckIfLectureExists(lecture))
            {
                Course course = db.Courses.Where(c => c.CourseId == CourseId).SingleOrDefault();
                ApplicationUser teacher = db.Users.Find(course.Teacher.Id);
                List<Lecture> lectures = course.Lectures.ToList();
                try
                {
                    lecture.Filename = Path.GetFileName(lecture.LectureFile.FileName);
                    string fileName = Path.Combine(Server.MapPath("~/Users/" + teacher.UserName + "/" + course.CourseName + "/Lectures/"), lecture.Filename);
                    lecture.LectureFile.SaveAs(fileName);

                    lecture.Path = fileName;
                    lecture.Course = course;
                    db.Lectures.Add(lecture);
                    db.SaveChanges();
                    return RedirectToAction("CourseHome", "Courses", course.CourseId);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /*public ActionResult Create(int courseid)
        {
            Lecture lecture = new Lecture();
            lecture.Course.CourseId = courseid;
            return View(lecture);
        }*/

        public ActionResult ListLectures(int? courseid)
        {
            ApplicationUser teacher = db.Users.Find(User.Identity.GetUserId());
            if (courseid != null)
            {
                Course course = db.Courses.Where(c => c.CourseId == courseid).SingleOrDefault();
                List<Lecture> lectures = course.Lectures.ToList();

                ViewBag.Id = courseid;

                return View(lectures);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecture lecture = db.Lectures.Find(id);
            if (lecture == null)
            {
                return HttpNotFound();
            }
            return View(lecture);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lecture lecture = db.Lectures.Include(l => l.Course).SingleOrDefault(l => l.LectureId == id);
            try
            {
                db.Lectures.Remove(lecture);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                throw e;
            }
            return RedirectToAction("CourseHome", "Courses");
        }

        public ActionResult Error()
        {
            return View();
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