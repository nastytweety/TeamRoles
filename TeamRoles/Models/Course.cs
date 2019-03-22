using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamRoles.Models
{
    public class Course
    {
        public Course()
        {
            this.ApplicationUsers = new HashSet<ApplicationUser>();
            this.Enrollments = new HashSet<Enrollment>();
            this.Assignments = new HashSet<Assignment>();
        }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string CoursePic { get; set; }
        public string TeacherName { get; set; }
        public string TeacherId { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}