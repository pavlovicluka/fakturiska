using Fakturiska.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Web.Mvc;

namespace Fakturiska.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            var identity = (ClaimsIdentity)User.Identity;
            return View(new UserModel(int.Parse(identity.GetUserId())));
        }
    }
}