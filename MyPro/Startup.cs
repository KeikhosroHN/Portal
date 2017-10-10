using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyPro.Startup))]
namespace MyPro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
