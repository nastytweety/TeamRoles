using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamRoles.Models;

namespace TeamRoles.Repositories
{
    public class UserRepository
    {
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
    }
}