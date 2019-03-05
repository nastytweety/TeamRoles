using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamRoles.Models
{
    public class Course
    {
        public Course()
        {
            this.ApplicationUsers = new HashSet<ApplicationUser>();
        }

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string CoursePic { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}