using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Business.DTOs;
using Fakturiska.Models;

namespace Fakturiska.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        public ActionResult Users()
        {
            return View();
        }

        public ActionResult TableUsers()
        {
            return PartialView("_TableUsers", UserModel.GetUsers());
        }

        public ActionResult TableUsersWaiting()
        {
            return PartialView("_TableUsersWaiting", UserModel.GetUsersWaiting());
        }

        [HttpGet]
        public ActionResult CreateUserWithoutPassword()
        {
            return PartialView("_CreateUserWithoutPassword");
        }

        [HttpPost]
        public ActionResult CreateUserWithoutPassword(UserModelWithoutPassword user)
        {
            if (ModelState.IsValid)
            {
                UserLogic.CreateUserWithoutPassword(new UserDTO
                {
                    Email = user.Email,
                    RoleId = (int)user.Role + 1,
                });
                return PartialView("_TableUsersWaiting", UserModel.GetUsersWaiting());
            }
            return PartialView("_CreateUserWithoutPassword", user);
        }

        [HttpPost]
        public ActionResult EditUserEmail(string value, string pk)
        {
            UserLogic.EditUserEmail(new UserDTO
            {
                UserGuid = new Guid(pk),
                Email = value,
            });
            return Json("Succeed");
        }

        [HttpPost]
        public ActionResult EditUserRole(string value, string pk)
        {
            UserLogic.EditUserRole(new UserDTO
            {
                UserGuid = new Guid(pk),
                RoleId = int.Parse(value),
            });
            return Json("Succeed");
        }

        [HttpPost]
        public ActionResult DeleteUser(Guid id)
        {
            UserLogic.DeleteUser(id);
            return Json("Succeed");
        } 
    }
}