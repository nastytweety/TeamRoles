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
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using TeamRoles.Models;
using TeamRoles.Repositories;
using System.Data.Entity.Validation;

namespace TeamRoles.Controllers
{
    [Authorize]
    public class AssignmentsController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> _userManager;

        public AssignmentsController()
        {
            db = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult CreateAssignment(Assignment assignment,int CourseId)
        {
            CoursesRepository repository = new CoursesRepository();
            Course course = db.Courses.Where(c => c.CourseId == CourseId).SingleOrDefault();
            if (!repository.CheckIfAssignmentExists(assignment,CourseId))
            {
                
                ApplicationUser teacher = db.Users.Find(course.Teacher.Id);
                List<Assignment> assignments = course.Assignments.ToList();

                assignment.Filename = Path.GetFileName(assignment.AssignmentFile.FileName);
                string fileName = Path.Combine(Server.MapPath("~/Users/" + teacher.UserName + "/" + course.CourseName), assignment.Filename);
                assignment.AssignmentFile.SaveAs(fileName);
                assignment.Path = fileName;
                assignment.DueDate = DateTime.Now; 
                var path = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Users\\" + teacher.UserName + "\\" + course.CourseName + "\\Submits\\" + assignment.AssignmentName);
                DirectoryInfo di = Directory.CreateDirectory(path.ToString());

                assignment.Course = course;
                db.Assignments.Add(assignment);
                db.SaveChanges();
                return RedirectToAction("CourseHome", "Courses", course.CourseId);
            }
            else
            {
                TempData["Error"] = "Assignment already exists! Try again";
                return RedirectToAction("CourseHome", "Courses", course);
            }
        }

        public ActionResult ListAssignments(int? courseid)
        {
            if(courseid!=null)
            {
                Course course = db.Courses.Where(c => c.CourseId == courseid).SingleOrDefault();
                List<Assignment> assignments = course.Assignments.ToList();
                ViewBag.CourseId = course.CourseId;
                return View(assignments);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }

//        public ActionResult UploadAnswear()
//        {
//            return View();
//        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assignment assignment = db.Assignments.Include(c => c.Course).Where(c => c.AssignmentId == id).SingleOrDefault();
            try
            {
                db.Assignments.Remove(assignment);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                throw e;
            }

            return RedirectToAction("CourseHome", "Courses");
        }

        public ActionResult Edit([Bind(Include = "AssignmentId,CourseId,Points,DueDate,AssignmentName")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListAssignments","Assignments",new { coursename = assignment.Course.CourseName });
            }
            return View(assignment);
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