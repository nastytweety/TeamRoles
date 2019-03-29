using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamRoles.Models
{
    public class SetGrade
    {
        public Course course { get; set; }
        public List<ApplicationUser> students { get; set; }
        public List<double> grades { get; set; }
    }
}