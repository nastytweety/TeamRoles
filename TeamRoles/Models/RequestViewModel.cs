using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamRoles.Models
{
    public class RequestViewModel
    {
        public RequestViewModel(int reqid,string id1,string id2, int courseid,string reqtype)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            this.ReqId = reqid;
            this.User1 = db.Users.Find(id1);
            this.User2 = db.Users.Find(id2);
            this.Course = db.Courses.Find(courseid);
            this.Type = reqtype;
        }

        public int ReqId { get; set; }
        public ApplicationUser User1 { get; set; }
        public ApplicationUser User2 { get; set; }
        public Course Course { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }
    }
}