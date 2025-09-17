using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(Demirbaş_Yönetim_sistemi.Startup))]
namespace Demirbaş_Yönetim_sistemi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
         
        }
    }
}
