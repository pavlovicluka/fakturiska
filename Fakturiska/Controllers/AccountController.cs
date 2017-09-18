﻿using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Business.DTOs;
using Fakturiska.Models;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Net;
using System.Web.Helpers;

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
                    new Claim(ClaimTypes.Role, user.RoleName),
                }, DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = true }, ident);
                return RedirectToAction("Invoices", "Invoice");
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Login");
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
                Email = model.Email,
                Password = model.Password,
                RoleId = (int)model.Role,
            });
            return RedirectToAction("Login");
        }

        public ActionResult SetPassword()
        {
            string userGuid = WebUtility.HtmlDecode(Request.QueryString["id"]);
            if(userGuid != null)
            {
                return View(new UserModel(userGuid));
            }
            return RedirectToAction("Invoices", "Invoice");
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

        public JsonResult GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)User.Identity;
                return Json(new UserModel(int.Parse(identity.GetUserId())));
            }
            return null;
        }
    }
}