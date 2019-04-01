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
            this.Lectures = new HashSet<Lecture>();
            this.Enrollments = new HashSet<Enrollment>();
            this.Assignments = new HashSet<Assignment>();
        }

        [Key]
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Course Name: ")]
        public string CourseName { get; set; }

        [Required]
        [Display(Name = "Course Description: ")]
        public string CourseDescription { get; set; }

        [Display(Name = "Course Picture: ")]
        public string CoursePic { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual ApplicationUser Teacher { get; set; }
    }
}