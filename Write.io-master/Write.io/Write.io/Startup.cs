using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Write.io.Startup))]
namespace Write.io
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
