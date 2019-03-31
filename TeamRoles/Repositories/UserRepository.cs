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


        public void DeleteStudent(ApplicationUser pstudent)
        {
            try
            {
                ApplicationUser student = db.Users.Find(pstudent.Id);
                CoursesRepository repository = new CoursesRepository();
                DeleteAllMessages(student);
                DeleteAllPosts(student);
                repository.DeleteCoursesEnrollments(student);
                db.Users.Remove(student);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}