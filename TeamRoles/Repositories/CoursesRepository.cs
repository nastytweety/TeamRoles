using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamRoles.Models;

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
    }
}