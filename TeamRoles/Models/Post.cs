using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamRoles.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string PostText { get; set; }
        public DateTime PostDate { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}