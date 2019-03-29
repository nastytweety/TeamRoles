using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamRoles.Models;
using System.Data.Entity;
using System.IO;

namespace TeamRoles.Repositories
{
    public class CoursesRepository
    {
        private ApplicationDbContext db;

        public CoursesRepository()
        {
            db = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets a Course's id since many teachers can have the same coursename
        /// </summary>
        /// <param name="coursename">The name of the course</param>
        /// <param name="teachername">The name of the teacher</param>
        /// <returns>an int representing the id of a course</returns>
        public int FindCourseId(string coursename, string teachername)
        {
            ApplicationUser teacher = db.Users.Where(u => u.UserName == teachername).SingleOrDefault();
            Course course = teacher.Courses.Where(c => c.CourseName == coursename).SingleOrDefault();
            return course.CourseId;
        }

        /// <summary>
        /// Removes all Lectures from a Course
        /// </summary>
        /// <param name="course">The Course to be Removed all Lectures</param>
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

        /// <summary>
        /// Removes all assignments from a Course
        /// </summary>
        /// <param name="course">The course to be removed all the Assignments</param>
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

        /// <summary>
        /// Creates and fills a teacherviewmodel
        /// </summary>
        /// <param name="id">the teacher id</param>
        /// <returns>the model</returns>
        public TeacherViewModel FillTheTeacherViewModel(string id)
        {
            TeacherViewModel model = new TeacherViewModel();
            ApplicationUser teacher = db.Users.Find(id);
            UserRepository repository = new UserRepository();

            model.Teacher = teacher;
            model.Courses = teacher.Courses.ToList();
            model.TotalLessons = teacher.Courses.Count();
            model.TotalStudents = repository.GetTotalStudents(teacher);
            return model;
        }

        /// <summary>
        /// Creates and saves to database a join request
        /// </summary>
        /// <param name="student">the student</param>
        /// <param name="course">the course</param>
        public void CreateJoinRequest(ApplicationUser student, Course course)
        {
            ApplicationUser teacher = course.Teacher;
            GenericRequest req = new GenericRequest();
            req.User1id = teacher.Id;
            req.User2id = student.Id;
            req.Courseid = course.CourseId;
            req.Type = "JoinCourse";
            req.ApplicationUser = teacher;
            teacher.Requests.Add(req);
            try
            {
                db.Requests.Add(req);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveCourse(int? id,ApplicationUser student)
        {
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
        }
    }
}