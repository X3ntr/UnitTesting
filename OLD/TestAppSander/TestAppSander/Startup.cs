using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestAppSander.Startup))]
namespace TestAppSander
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
