using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Business.DTOs;
using Fakturiska.Models;
using Fakturiska.Business.Enumerations;

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
            return PartialView("_TableUsers", UserWithoutPasswordModel.GetUsers());
        }

        public ActionResult TableUsersWaiting()
        {
            return PartialView("_TableUsersWaiting", UserWithoutPasswordModel.GetUsersWaiting());
        }

        [HttpGet]
        public ActionResult CreateUserWithoutPassword()
        {
            return PartialView("_CreateUserWithoutPassword", new UserWithoutPasswordModel());
        }

        [HttpPost]
        public ActionResult CreateUserWithoutPassword(UserWithoutPasswordModel user)
        {
            if (ModelState.IsValid)
            {
                var response = UserLogic.CreateUserWithoutPassword(new UserDTO
                {
                    Email = user.Email,
                    Role = (RoleEnum)user.Role,
                });
                if(response == "success")
                {
                    return PartialView("_TableUsersWaiting", UserWithoutPasswordModel.GetUsersWaiting());
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Korisnik sa istim email-om je već kreiran!");
                }
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
            return Json("success");
        }

        [HttpPost]
        public ActionResult EditUserRole(string value, string pk)
        {
            UserLogic.EditUserRole(new UserDTO
            {
                UserGuid = new Guid(pk),
                Role = (RoleEnum)int.Parse(value),
            });
            return Json("success");
        }

        [HttpPost]
        public ActionResult DeleteUser(Guid id)
        {
            UserLogic.DeleteUser(id);
            return Json("success");
        } 
    }
}