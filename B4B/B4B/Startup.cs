using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(B4B.Startup))]
namespace B4B
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
