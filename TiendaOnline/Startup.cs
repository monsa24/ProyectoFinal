using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TiendaOnline.Startup))]
namespace TiendaOnline
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
