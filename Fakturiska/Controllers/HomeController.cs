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
            return RedirectToAction("Invoices", "Invoice");
        }
    }
}