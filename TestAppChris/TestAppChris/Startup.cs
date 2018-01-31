using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestAppChris.Startup))]
namespace TestAppChris
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
