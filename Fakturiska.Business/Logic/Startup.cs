using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.ModelBinding;

[assembly: OwinStartup(typeof(Fakturiska.Business.Logic.Startup))]
namespace Fakturiska.Business.Logic
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            OwinConfig.ConfigureAuth(app);
        }
    }
}
