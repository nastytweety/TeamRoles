using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamRoles.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public HomeIndexViewModel()
        {
            this.Posts = new List<Post>();
            this.User = new ApplicationUser();
            this.TotalLessons = 0;
            this.TotalLessons = 0;
        }
        public ApplicationUser User { get; set; }
        public List<Post> Posts { get; set; }
        public int TotalLessons { get; set; }
        public int TotalStudents { get; set; }
    }
}