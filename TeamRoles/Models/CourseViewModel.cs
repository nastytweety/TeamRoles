using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamRoles.Models
{
    public class CourseViewModel 
    {
        public CourseViewModel()
        {
            this.Courses = new List<Course>();
            this.Grades = new List<double>();
        }
        public List<Course> Courses { get; set; }
        public ApplicationUser Teacher { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CoursePic { get; set; }
        public string CourseDescription { get; set; }
        public List<double> Grades { get; set; }
    }
}