using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeamRoles.Models;
using TeamRoles.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;


namespace TeamRoles.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private ApplicationDbContext db;

        public CoursesController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            ApplicationUser student = db.Users.Find(User.Identity.GetUserId());
            List<Course> Courses = new List<Course>();
            Courses = student.Courses.ToList();

            return View(Courses);
        }

        public ActionResult Index_StudentId(string id)
        {
            ApplicationUser student = db.Users.Find(id);
            return View(student.Courses.ToList());
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Admin_Index()
        {
            return View(db.Courses.ToList());
        }

        public ActionResult Index_Selected()
        {
            ApplicationUser student = db.Users.Find(User.Identity.GetUserId());
            var model = new CourseViewModel
            {
                Courses = student.Enrollments.Select(e => e.Course).ToList()
            };
            return View(model);
        }

        public ActionResult Index_ToSelect(string id)
        {
            if(id!=null)
            {
                ApplicationUser student = db.Users.Find(User.Identity.GetUserId());                
                CoursesRepository repository = new CoursesRepository();
                List<Course> selectedcourses = student.Enrollments.Select(e => e.Course).ToList();
                TeacherViewModel model = repository.FindAvailableCourses(id,selectedcourses);
                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
        public ActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                    ApplicationUser teacher = db.Users.Find(User.Identity.GetUserId());
                    List<Course> courses = teacher.Courses.ToList();

                    foreach (var c in courses)
                    {
                        if (c.CourseName == course.CourseName)
                        {
                            return RedirectToAction("Error");
                        }
                    }
                    course.Teacher = db.Users.Find(User.Identity.GetUserId());
                    course.CoursePic = Path.GetFileName(course.ImageFile.FileName);
                    try
                    {
                        db.Courses.Add(course);
                        db.SaveChanges();
                        var path = teacher.Path + "\\" + course.CourseName;
                        DirectoryInfo di = Directory.CreateDirectory(path.ToString());
                        path = teacher.Path + "\\" + course.CourseName + "\\Submits\\";
                        di = Directory.CreateDirectory(path.ToString());
                        path = teacher.Path + "\\" + course.CourseName + "\\Lectures\\";
                        di = Directory.CreateDirectory(path.ToString());
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }


                    string fileName = Path.Combine(Server.MapPath("~/Users/" + teacher.UserName + "/" + course.CourseName + "/"), course.CoursePic);
                    course.ImageFile.SaveAs(fileName);

                    return RedirectToAction("Index");
            }
            return View(course);
        }

        public ActionResult Join(int? id)
        {
            if (id != null)
            {
                Course course = db.Courses.Find(id);
                ApplicationUser student = db.Users.Find(User.Identity.GetUserId());
                CoursesRepository repository = new CoursesRepository();
                repository.CreateJoinRequest(student, course);
                return RedirectToAction("RequestSent", "Courses");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,CourseName,CourseDescription,ImageFile")] Course course, HttpPostedFileBase ImageFile)
        {
            if(course.CourseId!=0)
            { 

                    Course coursetoupdate = db.Courses.Find(course.CourseId);

                    if (course.ImageFile != null)
                    {
                        course.CoursePic = Path.GetFileName(course.ImageFile.FileName);
                        ApplicationUser teacher = coursetoupdate.Teacher;
                        string fileName = Path.Combine(Server.MapPath("~/Users/" + teacher.UserName + "/" + course.CourseName + "/"), course.CoursePic);
                        course.ImageFile.SaveAs(fileName);
                        coursetoupdate.CoursePic = course.CoursePic;
                    }

                    if(course.CourseName!= null)
                    {
                        coursetoupdate.CourseName = course.CourseName;
                    }
                   
                    if(course.CourseDescription!=null)
                    {
                        coursetoupdate.CourseDescription = course.CourseDescription;
                    }

                    try
                    {
                        db.Entry(coursetoupdate).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                
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
            ApplicationUser teacher = course.Teacher;
            CoursesRepository repository = new CoursesRepository();
            try
            {
                var path = teacher.Path + "\\" + course.CourseName;
                Directory.Delete(path.ToString(), true);
                repository.RemoveAssignments(course);
                repository.RemoveLectures(course);
                repository.DeleteCoursesEnrollments(course);
                db.Courses.Remove(course);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return RedirectToAction("Index");
        }
        

        public ActionResult CourseHome(int? id)
        {
            if(id!=null)
            {
                Course course = db.Courses.Find(id);
                CourseViewModel model = new CourseViewModel();
                model.CourseName = course.CourseName;
                model.CoursePic = course.CoursePic;
                model.CourseId = course.CourseId;
                model.CourseDescription = course.CourseDescription;
                model.Courses.Add(course);
                model.Teacher = course.Teacher;
                model.Enrollments = course.Enrollments.ToList();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult StudentRemoveCourse(int? id)
        {
            if(id!=null)
            {
                ApplicationUser student = db.Users.Find(User.Identity.GetUserId());
                CoursesRepository repository = new CoursesRepository();
                repository.RemoveCourse(id, student);
                return RedirectToAction("Index_Selected");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
        }

        public ActionResult CourseGrades(string coursename, string teacherid)
        {
            if (coursename != null & teacherid != null)
            {
                List<Course> courselist = db.Courses.Where(t => t.Teacher.Id == teacherid).ToList();
                Course course = courselist.Where(c => c.CourseName == coursename).SingleOrDefault();
                return View(course.Enrollments.ToList());
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult RequestSent()
        {
            return View();
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
