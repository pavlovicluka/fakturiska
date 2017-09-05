using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Business.DTOs;
using Fakturiska.Models;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Fakturiska.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            UserDTO user = UserLogic.AuthorizeUser(model.Email, model.Password);
            if (user != null)
            {
                var ident = new ClaimsIdentity(
                new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                }, DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);
                return View();
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignUp()
        {
            return View(new UserModel());
        }

        [HttpPost]
        public ActionResult SignUp(UserModel model)
        {

            UserLogic.CreateUser(new UserDTO
            {
                UserGuid = Guid.NewGuid(),
                Email = model.Email,
                Password = model.Password,
                RoleId = (int)model.Role,
            });
            return RedirectToAction("Login");
        }

        public ActionResult SetPassword()
        {
            string userGuid = Request.QueryString["id"];
            if(userGuid != null)
            {
                return View(new UserModel(userGuid));
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SetPassword(UserModel model)
        {
            UserLogic.SetPassword(new UserDTO
            {
                UserGuid = model.UserGuid,
                Password = model.Password,
            });
            return RedirectToAction("Login");
        }
    }
}