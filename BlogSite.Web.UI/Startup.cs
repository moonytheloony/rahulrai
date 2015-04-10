using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogSite.Web.UI.Startup))]
namespace BlogSite.Web.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
