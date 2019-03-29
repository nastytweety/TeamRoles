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
using System.IO;


namespace TeamRoles.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private ApplicationDbContext db;
        //private UserManager<ApplicationUser> _userManager;

        public CoursesController()
        {
            db = new ApplicationDbContext();
            //_userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        public ActionResult Index()
        {
            ApplicationUser student = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

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
            ApplicationUser student = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            CourseViewModel model = new CourseViewModel();
            List<Course> Courses = new List<Course>();
            model.Courses = student.Enrollments.Select(e => e.Course).ToList();
            return View(model);
        }

        public ActionResult Index_ToSelect(string id)
        {
                ApplicationUser teacher = db.Users.Find(id);
                ApplicationUser student = db.Users.Find(User.Identity.GetUserId());
                List<Course> TeacherCourses = teacher.Courses.ToList();
                List<Course> StudentCourses = student.Enrollments.Select(e => e.Course).ToList();
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
                    if (found == false)
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

        public ActionResult Join(int id)
        {
                Course course = db.Courses.Find(id);
                ApplicationUser teacher = course.Teacher;
                ApplicationUser student = db.Users.Find(User.Identity.GetUserId());

                GenericRequest req = new GenericRequest();
                req.User1id = teacher.Id;
                req.User2id = student.Id;
                req.Courseid = course.CourseId;
                req.Type = "JoinCourse";
                req.ApplicationUser = teacher;
                teacher.Requests.Add(req);
                db.Requests.Add(req);
                db.SaveChanges();

            return RedirectToAction("RequestSent", "Courses");
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
        
        
        public ActionResult Delete(int id)
        {
            Course course = db.Courses.Find(id);
            ApplicationUser teacher = course.Teacher;
            if (course == null)
            {
                return HttpNotFound();
            }
            try
                {
                    var path = teacher.Path + "\\" + course.CourseName;
                    Directory.Delete(path.ToString(), true);
                    RemoveAssignments(course);
                    RemoveLectures(course);
                    db.Courses.Remove(course);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
                return RedirectToAction("Index");
        }
        

        public ActionResult Error()
        {
            return View();
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
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public void RemoveAssignments(Course course)
        {
            List<Assignment> listofassignments = new List<Assignment>();
            listofassignments = db.Assignments.Where(c => c.Course.CourseId == course.CourseId).ToList();
            foreach (var assignment in listofassignments)
            {

                try
                {
                    db.Assignments.Remove(assignment);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void RemoveLectures(Course course)
        {
            List<Lecture> listoflectures = new List<Lecture>();
            listoflectures = db.Lectures.Where(c => c.Course.CourseId == course.CourseId).ToList();
            foreach (var lecture in listoflectures)
            {
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
        }

        public ActionResult StudentRemoveCourse(int? id)
        {
            if(id!=null)
            {
                    ApplicationUser student = db.Users.Find(User.Identity.GetUserId());
                    Course course = db.Courses.Find(id);
                    student.Courses.Remove(course);
                    try
                    {
                        db.Entry(student).State = EntityState.Modified;
                        db.SaveChanges();
                        var path = student.Path + "\\" + course.CourseName;
                        Directory.Delete(path.ToString(), true);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    return RedirectToAction("Index_Selected");
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
