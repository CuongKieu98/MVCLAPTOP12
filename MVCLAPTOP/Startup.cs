using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCLAPTOP.Startup))]
namespace MVCLAPTOP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
