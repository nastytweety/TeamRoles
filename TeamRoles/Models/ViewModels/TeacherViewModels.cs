using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamRoles.Models
{
    public class TeacherViewModel
    {
        public TeacherViewModel()
        {
            this.Courses = new List<Course>();
            this.TotalLessons = 0;
            this.TotalStudents = 0;
        }
        public List<Course> Courses { get; set; }
        public ApplicationUser Teacher { get; set; }
        public int TotalLessons { get; set; }
        public int TotalStudents { get; set; }
    }
}