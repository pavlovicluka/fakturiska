using System;
using System.Web.Mvc;

namespace Fakturiska.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}