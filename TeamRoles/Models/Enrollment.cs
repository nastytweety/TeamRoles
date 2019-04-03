using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamRoles.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public string UserId { get; set; }
        public double Grade { get; set; }
        public int Absences { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}