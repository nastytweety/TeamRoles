using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamRoles.Models
{
    public class Index_SelectedViewModel 
    {
        public Index_SelectedViewModel()
        {
            this.Courses = new List<Course>();
            this.Grades = new List<double>();
        }
        public List<Course> Courses { get; set; }
        public ApplicationUser Teacher { get; set; }
        public string CourseName { get; set; }
        public string CoursePic { get; set; }
        public string CourseDescription { get; set; }
        public List<double> Grades { get; set; }
    }
}