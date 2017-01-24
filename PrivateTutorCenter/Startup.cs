using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PrivateTutorCenter.Startup))]
namespace PrivateTutorCenter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
