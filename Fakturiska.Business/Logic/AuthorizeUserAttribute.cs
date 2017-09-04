using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Fakturiska.Business.Logic
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            string privilegeLevels = string.Join("", UserLogic.GetUserRights(httpContext.User.Identity.Name.ToString())); // Call another method to get rights of the user from DB

            if (privilegeLevels.Contains(this.AccessLevel))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Home",
                                action = "Login"
                            })
                        );
        }

    }
}
