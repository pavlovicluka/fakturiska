using Fakturiska.App_Start;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fakturiska.App_Start.Startup))]
namespace Fakturiska.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            JobScheduler.ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}