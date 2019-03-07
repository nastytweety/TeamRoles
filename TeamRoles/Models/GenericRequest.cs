using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamRoles.Models
{
    public class GenericRequest
    {
        public GenericRequest()
        {
            this.ApplicationUsers = new HashSet<ApplicationUser>();
        }

        [Key]
        public int ReqId { get; set; }
        public string User1id { get; set; }
        public string User2id { get; set; }
        public int Courseid { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}