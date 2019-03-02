using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeamRoles.Startup))]
namespace TeamRoles
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
