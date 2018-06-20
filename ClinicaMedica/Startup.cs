using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClinicaMedica.Startup))]
namespace ClinicaMedica
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
