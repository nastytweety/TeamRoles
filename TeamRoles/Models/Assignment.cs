using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamRoles.Models
{
    public class Assignment
    {
        public Assignment()
        {
            this.Courses = new HashSet<Course>();
        }
        [Key]
        public int AssignmentId { get; set; }
        public string AssignmentName { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
        public DateTime DueDate { get; set; }
        public int Points { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        [NotMapped]
        public HttpPostedFileBase AssignmentFile { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}