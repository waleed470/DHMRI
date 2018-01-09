using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DHMRice.Startup))]
namespace DHMRice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
