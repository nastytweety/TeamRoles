using System.IO;
using System.Data.Entity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace TeamRoles.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Courses = new HashSet<Course>();
            Posts = new HashSet<Post>();
            Requests = new HashSet<GenericRequest>();
            Enrollments = new HashSet<Enrollment>();
            Children = new HashSet<Child>();
            MessagesSent = new List<Message>();
            MessagesReceived = new List<Message>();
        }
        public string Path { get; set; }
        public string ProfilePic { get; set; }
        [InverseProperty("Sender")]
        public ICollection<Message> MessagesSent { get; set; }
        [InverseProperty("Receiver")]
        public ICollection<Message> MessagesReceived { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
       

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<GenericRequest> Requests { get; set; }
        public virtual ICollection<Child> Children { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("ProfilePic", this.ProfilePic));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<GenericRequest> Requests { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}