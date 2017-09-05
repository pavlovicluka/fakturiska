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
            return View(UserModel.GetAllUsers());
        }

        [HttpPost]
        public ActionResult EditUser(string value, string pk)
        {
            UserLogic.EditUser(new UserDTO
            {
                UserGuid = new Guid(pk),
                Email = value,
            });
            return RedirectToAction("Users");
        }

        public ActionResult DeleteUser(Guid id)
        {
            UserLogic.DeleteUser(id);
            return RedirectToAction("Users");
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
            return RedirectToAction("Users");
        }
    }
}