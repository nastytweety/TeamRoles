using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using TeamRoles.Models;
using Owin;
using System.Security.Claims;
using System.IO;

[assembly: OwinStartupAttribute(typeof(TeamRoles.Startup))]
namespace TeamRoles
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);
            createRolesandUsers();
            app.MapSignalR();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));


            // In Startup iam creating first Admin Role and creating a default Admin User 
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin role
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website				

                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    ProfilePic = "46768452_10214838176481822_2515122708119814144_n.jpg",
                    BirthDay = new System.DateTime(1990, 1, 1),
                    Validated = true
                };

                string userPWD = "Admin//1235";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                    var dir = new System.IO.DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory + "Users\\Admin");
                    DirectoryInfo di = Directory.CreateDirectory(dir.ToString());
                }
            }

            // creating Creating Manager role 
            if (!roleManager.RoleExists("Teacher"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Teacher";
                roleManager.Create(role);

            }

            // creating Creating Employee role 
            if (!roleManager.RoleExists("Student"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Student";
                roleManager.Create(role);

            }

            if (!roleManager.RoleExists("Parent"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Parent";
                roleManager.Create(role);

            }
        }
    }
}
