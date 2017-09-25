using Fakturiska.Business.Enumerations;
using Fakturiska.Business.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fakturiska.Models
{
    public class UserWithoutPasswordModel
    {
        public Guid UserGuid { get; set; }

        [DisplayName("Email")]
        [Required()]
        [EmailAddress]
        public String Email { get; set; }

        [DisplayName("Rola")]
        [Required()]
        public RoleEnum Role { get; set; }

        public UserWithoutPasswordModel()
        {

        }

        public static IEnumerable<UserWithoutPasswordModel> GetUsers()
        {
            return UserLogic.GetUsers().Select(user => new UserWithoutPasswordModel
            {
                UserGuid = user.UserGuid,
                Email = user.Email,
                Role = user.Role,
            });
        }

        public static IEnumerable<UserWithoutPasswordModel> GetUsersWaiting()
        {
            return UserLogic.GetUsersWaiting().Select(user => new UserWithoutPasswordModel
            {
                UserGuid = user.UserGuid,
                Email = user.Email,
                Role = user.Role,
            });
        }
    }
}