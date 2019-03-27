using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using TeamRoles.Models;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TeamRoles.Controllers
{
    public class LecturesController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(Lecture lecture)
        {
            if(!CheckIfLectureExists(lecture))
            {
                using (var db = new ApplicationDbContext())
                {
                    ApplicationUser teacher = db.Users.Find(User.Identity.GetUserId());
                    Course course = teacher.Courses.Where(c => c.CourseId == lecture.Course.CourseId).SingleOrDefault();
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
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Create(int courseid)
        {
            Lecture lecture = new Lecture();
            lecture.Course.CourseId = courseid;
            return View(lecture);
        }

        public ActionResult ListLectures(int? courseid)
        {
            using (var db = new ApplicationDbContext())
            {
                ApplicationUser teacher = db.Users.Find(User.Identity.GetUserId());
                if (courseid != null)
                {
                    Course course = db.Courses.Where(c => c.CourseId == courseid).SingleOrDefault();
                    List<Lecture> lectures = course.Lectures.ToList();
                    return View(lectures);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
        }

        public bool CheckIfLectureExists(Lecture lecture)
        {
            using (var db = new ApplicationDbContext())
            {
                ApplicationUser teacher = db.Users.Find(User.Identity.GetUserId());
                Course course = teacher.Courses.Where(c => c.CourseId == lecture.Course.CourseId).SingleOrDefault();
                List<Lecture> lectures = course.Lectures.ToList();
                foreach (var l in lectures)
                {
                    if (l.LectureName == lecture.LectureName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new ApplicationDbContext())
            {
                Lecture lecture = db.Lectures.Find(id);
                if (lecture == null)
                {
                    return HttpNotFound();
                }
                return View(lecture);
            }
        }


        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                Lecture lecture = db.Lectures.Find(id);
                try
                {
                    db.Lectures.Remove(lecture);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return RedirectToAction("CourseHome", "Courses");
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}