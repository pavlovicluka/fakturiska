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

        [HttpPost]
        public ActionResult EditUser(string value, string pk)
        {
            UserLogic.EditUser(new UserDTO
            {
                UserGuid = new Guid(pk),
                Email = value,
            });
            return Json("Succeed");
        }

        [HttpPost]
        public ActionResult DeleteUser(Guid id)
        {
            UserLogic.DeleteUser(id);
            return Json("Succeed");
        }

        [HttpPost]
        public ActionResult CreateUserWithoutPassword(string email, string role)
        {
            UserLogic.CreateUserWithoutPassword(new UserDTO
            {
                UserGuid = Guid.NewGuid(),
                Email = email,
                RoleId = int.Parse(role),
            });
            return Json("Succeed");
        }
    }
}