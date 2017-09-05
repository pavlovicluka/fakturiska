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

        public ActionResult Users()
        {
            return View(UserModel.GetAllUsers());
        }
    }
}