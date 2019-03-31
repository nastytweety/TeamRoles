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
        /// Finds all the available courses from a teacher that a student has not selected
        /// </summary>
        /// <param name="id">the teacher id</param>
        /// <returns>the model</returns>
        public TeacherViewModel FindAvailableCourses(string id, List<Course> selectedcourses)
        {
            TeacherViewModel model = new TeacherViewModel();
            UserRepository repository = new UserRepository();

            ApplicationUser teacher = db.Users.Find(id);

            List<Course> teachercourses = teacher.Courses.ToList();
            List<Course> availabecourses = teacher.Courses.ToList();
            //List<Course> availabecourses = teacher.Courses.Except(selectedcourses).ToList();
            foreach (var scourse in selectedcourses)
            {
                foreach(var tcourse in teachercourses)
                {
                    if(tcourse.CourseId == scourse.CourseId)
                    {
                        availabecourses.Remove(tcourse);
                    }
                }
            }
            model.Teacher = teacher;
            model.Courses = availabecourses;
            model.TotalLessons = teacher.Courses.Count();
            model.TotalStudents = repository.GetTotalStudents(teacher);
            return model;
        }

        /// <summary>
        /// Creates and saves to database a join request
        /// </summary>
        /// <param name="student">the student</param>
        /// <param name="course">the course</param>
        public void CreateJoinRequest(ApplicationUser pstudent, Course course)
        {
            ApplicationUser student = db.Users.Find(pstudent.Id);
            ApplicationUser teacher = db.Users.Find(course.Teacher.Id);
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

        /// <summary>
        /// Removes the enrollment of a student in a course
        /// </summary>
        /// <param name="id">course id </param>
        /// <param name="pstudent">the student</param>
        public void RemoveCourse(int? id,ApplicationUser pstudent)
        {
            if(id!=null && pstudent!=null)
            {
                ApplicationUser student = db.Users.Find(pstudent.Id);
                Course course = db.Courses.Find(id);
                Enrollment enrol = db.Enrollments.Where(e => e.CourseId == id).SingleOrDefault();
                //student.Enrollments.Remove(enrol);
                try
                {
                    //db.Entry(student).State = EntityState.Modified;
                    db.Enrollments.Remove(enrol);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Deletes all courses from a teacher
        /// </summary>
        /// <param name="teacher">the teacher</param>
        public void DeleteAllCourses(ApplicationUser teacher)
        {
            if(teacher!=null)
            {
                List<Course> courselist = db.Courses.Where(c => c.Teacher.Id == teacher.Id).ToList();
                foreach (var course in courselist)
                {
                    try
                    {
                        db.Courses.Remove(course);
                        db.SaveChanges();
                    }
                    catch(Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// Deletes all student enrollments
        /// </summary>
        /// <param name="student">the student</param>
        public void DeleteCoursesEnrollments(ApplicationUser student)
        {
            if (student != null)
            {
                List<Enrollment> enrollist = db.Enrollments.Where(c => c.UserId == student.Id).ToList();
                foreach (var enrol in enrollist)
                {
                    try
                    {
                        db.Enrollments.Remove(enrol);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// Deletes all the enrollments from a course
        /// </summary>
        /// <param name="course">the course</param>
        public void DeleteCoursesEnrollments(Course course)
        {
            if (course != null)
            {
                List<Enrollment> enrollist = db.Enrollments.Where(c => c.CourseId == course.CourseId).ToList();
                foreach (var enrol in enrollist)
                {
                    try
                    {
                        db.Enrollments.Remove(enrol);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if an assignment exists
        /// </summary>
        /// <param name="assignment">The assignment</param>
        /// <param name="CourseId">The course id</param>
        /// <returns>boolean</returns>
        public bool CheckIfAssignmentExists(Assignment assignment, int CourseId)
        {
            Course course = db.Courses.Where(c => c.CourseId == CourseId).SingleOrDefault();
            List<Assignment> assignments = course.Assignments.ToList();
            foreach (var a in assignments)
            {
                if (a.AssignmentName == assignment.AssignmentName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a lecture exists
        /// </summary>
        /// <param name="lecture">The lecture</param>
        /// <returns>boolean</returns>
        public bool CheckIfLectureExists(Lecture lecture)
        {
            Course course = db.Courses.Where(c => c.CourseId == lecture.Course.CourseId).SingleOrDefault();
            List<Lecture> lectures = course.Lectures.ToList();
            foreach (var l in lectures)
            {
                if (l.LectureName == lecture.LectureName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}