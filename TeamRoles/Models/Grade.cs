using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamRoles.Models
{
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }
        public string Type { get; set; }
        public double NumericGrade { get; set; }

        public virtual Course Course{ get; set; }
    }
}