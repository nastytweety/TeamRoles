using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamRoles.Models
{
    public class Child
    {
        public Child()
        {
            this.Parent = new HashSet<ApplicationUser>();
        }
        [Key]
        public string Childid { get; set; }
        public virtual ICollection<ApplicationUser> Parent { get; set; }
    }
}