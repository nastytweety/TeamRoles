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
            this.Course = new Course();
        }
        [Key]
        public int AssignmentId { get; set; }

        [Display(Name = "Assignment Name: ")]
        public string AssignmentName { get; set; }

        [Display(Name = "Archive: ")]
        public string Filename { get; set; }

        [Display(Name = "Due Date: ")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Points: ")]
        public int Points { get; set; }

        public string Path { get; set; }
        [NotMapped]
        public HttpPostedFileBase AssignmentFile { get; set; }

        public virtual Course Course { get; set; }
    }
}