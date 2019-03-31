using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamRoles.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.IO;

namespace TeamRoles.Repositories
{
    public class UserRepository
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> _userManager;

        public UserRepository()
        {
            db = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        /// <summary>
        /// Gets the Distinct number of students that follow a teacher in his courses
        /// </summary>
        /// <param name="teacher">The teacher</param>
        /// <returns>an integer representing the number of students</returns>
        public int GetTotalStudents(ApplicationUser teacher)
        {
            List<string> list = new List<string>();
            if (teacher != null)
            {
                List<Course> courses = teacher.Courses.ToList();
                foreach (var course in courses)
                {
                    foreach (var enrol in course.Enrollments.ToList())
                    {
                        list.Add(enrol.User.Id);
                    }
                }
            }
            return list.Distinct().Count();
        }

        /// <summary>
        /// Deletes all user chat messages
        /// </summary>
        /// <param name="user">the user</param>
        public void DeleteAllMessages(ApplicationUser user)
        {
            if (user != null)
            {
                List<Message> messagelist = db.Messages.Where(c => c.Receiver.Id == user.Id).ToList();
                messagelist.AddRange(db.Messages.Where(c => c.Sender.Id == user.Id).ToList());

                foreach (var message in messagelist)
                {
                    try
                    {
                        db.Messages.Remove(message);
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
        /// Deletes all user posts
        /// </summary>
        /// <param name="user">the user</param>
        public void DeleteAllPosts(ApplicationUser user)
        {
            if (user != null)
            {
                List<Post> postslist = db.Posts.Where(c => c.ApplicationUser.Id == user.Id).ToList();

                foreach (var post in postslist)
                {
                    try
                    {
                        db.Posts.Remove(post);
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
        /// Checking if birthday provided matching a students birthday
        /// </summary>
        /// <param name="Id">the student id</param>
        /// <param name="BirthDay">the birthday provided</param>
        /// <returns></returns>
        public bool CheckIfBirthDaysMatch(string Id, DateTime BirthDay)
        {
            if (Id != null && BirthDay != null)
            {
                ApplicationUser student = db.Users.Find(Id);
                if (student.BirthDay == BirthDay)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Finds all students
        /// </summary>
        /// <returns>List of students</returns>
        public List<ApplicationUser> FindStudent()
        {
            List<ApplicationUser> Users = db.Users.ToList();
            List<ApplicationUser> usersInRole = new List<ApplicationUser>();

            foreach (var user in Users)
            {
                var isInRole = _userManager.IsInRole(user.Id, "Student");
                if (isInRole)
                {
                    usersInRole.Add(user);
                }
            }
            return usersInRole;
        }

        /// <summary>
        /// Finds all teachers
        /// </summary>
        /// <returns>List of teachers</returns>
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

        /// <summary>
        /// Deletes specific teacher
        /// </summary>
        /// <param name="pteacher">the teacher</param>
        public void DeleteTeacher(ApplicationUser pteacher)
        {
            try
            {
                ApplicationUser teacher = db.Users.Find(pteacher.Id);
                CoursesRepository repository = new CoursesRepository();
                repository.DeleteAllCourses(teacher);
                DeleteAllMessages(teacher);
                DeleteAllPosts(teacher);
                var path = teacher.Path;
                Directory.Delete(path.ToString(), true);
                db.Users.Remove(teacher);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Deletes specific student
        /// </summary>
        /// <param name="pstudent">the student</param>
        public void DeleteStudent(ApplicationUser pstudent)
        {
            try
            {
                ApplicationUser student = db.Users.Find(pstudent.Id);
                CoursesRepository repository = new CoursesRepository();
                DeleteAllMessages(student);
                DeleteAllPosts(student);
                DeleteAsChildren(student);
                repository.DeleteCoursesEnrollments(student);
                db.Users.Remove(student);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Delete a student's id from children table
        /// </summary>
        /// <param name="student">the student</param>
        public void DeleteAsChildren(ApplicationUser student)
        {
            Child toberemoved = db.Children.Find(student.Id);
            if (toberemoved!=null)
            {
                try
                {
                    db.Children.Remove(toberemoved);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Find all parents
        /// </summary>
        /// <returns>list of parents</returns>
        public List<ApplicationUser> FindParents()
        {
            List<ApplicationUser> Users = db.Users.ToList();
            List<ApplicationUser> usersInRole = new List<ApplicationUser>();

            foreach (var user in Users)
            {
                var isInRole = _userManager.IsInRole(user.Id, "Parent");
                if (isInRole)
                {
                    usersInRole.Add(user);
                }
            }
            return (usersInRole);
        }

        /// <summary>
        /// Delete specific parent
        /// </summary>
        /// <param name="pparent">the parent</param>
        public void DeleteParent(ApplicationUser pparent)
        {
            try
            {
                ApplicationUser parent = db.Users.Find(pparent.Id);
                CoursesRepository repository = new CoursesRepository();
                DeleteAllMessages(parent);
                DeleteAllPosts(parent);
                db.Users.Remove(parent);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Create a parent request
        /// </summary>
        /// <param name="parent">the parent</param>
        /// <param name="student">the student</param>
        /// <returns>true if success</returns>
        public bool CreateParentRequest(ApplicationUser parent,ApplicationUser student)
        {
            GenericRequest req = new GenericRequest();
            req.User1id = parent.Id;
            req.User2id = student.Id;
            req.Type = "ParentStudent";
            req.ApplicationUser = student;
            student.Requests.Add(req);
            try
            {
                db.Requests.Add(req);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }
    }
}