using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamRoles.Models
{
    public class SetGradeViewModel
    {
        public string StudentName { get; set; }
        public string TeacherName { get; set; }
        public string CourseName { get; set; }
        public double NumericGrade { get; set; }
        public int Absences { get; set; }
    }
}