using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Business.DTOs;
using Fakturiska.Models;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Net;

namespace Fakturiska.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
                return RedirectToAction("Invoices", "Invoice");
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            if(ModelState.IsValid)
            {
                UserDTO user = UserLogic.AuthorizeUser(model.Email, model.Password);
                if (user != null)
                {
                    var ident = new ClaimsIdentity(
                    new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    }, DefaultAuthenticationTypes.ApplicationCookie);

                    HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = true }, ident);

                    return RedirectToAction("Invoices", "Invoice");
                } else
                {
                    ModelState.AddModelError(string.Empty, "Pogresan email i/ili sifra");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult SignUp()
        {
            return View(new UserSetPasswordModel());
        }

        [HttpPost]
        public ActionResult SignUp(UserSetPasswordModel model)
        {
            UserLogic.CreateUser(new UserDTO
            {
                Email = model.Email,
                Password = model.Password,
                Role = model.Role,
            });
            return RedirectToAction("Login");
        }

        public ActionResult SetPassword()
        {
            string userGuid = WebUtility.HtmlDecode(Request.QueryString["id"]);
            if(userGuid != null)
            {
                return View(new UserSetPasswordModel(userGuid));
            }
            return RedirectToAction("Invoices", "Invoice");
        }

        [HttpPost]
        public ActionResult SetPassword(UserSetPasswordModel model)
        {
            if(ModelState.IsValid)
            {
                UserLogic.SetPassword(new UserDTO
                {
                    UserGuid = model.UserGuid,
                    Password = model.Password,
                });
                return RedirectToAction("Login");
            }
            return View(model);
        }
    }
}