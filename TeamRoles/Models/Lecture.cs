using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamRoles.Models
{
    public class Lecture
    {
        public Lecture()
        {
            this.Course = new Course();
        }
        [Key]
        public int LectureId { get; set; }
        public string LectureName { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
        public DateTime PostDate { get; set; }
        [NotMapped]
        public HttpPostedFileBase LectureFile { get; set; }

        public virtual Course Course { get; set; }
    }
}