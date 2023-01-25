using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(freshSite2.Startup))]
namespace freshSite2
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
