using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PlantDatabase.Startup))]
namespace PlantDatabase
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
