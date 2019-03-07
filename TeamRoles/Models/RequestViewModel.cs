using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamRoles.Models
{
    public class RequestViewModel
    {
        public RequestViewModel(int reqid,string id1,string id2, int courseid)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            this.ReqId = reqid;
            this.Teacher = db.Users.Find(id1);
            this.Student = db.Users.Find(id2);
            this.Course = db.Courses.Find(courseid);
        }

        public int ReqId { get; set; }
        public ApplicationUser Teacher { get; set; }
        public ApplicationUser Student { get; set; }
        public Course Course { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }
    }
}