using Fakturiska.Business.DTOs;
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
    public class UserModel
    {
        public Guid UserGuid { get; set; }
        [DisplayName("Email")]
        [Required()]
        public String Email { get; set; }
        [DisplayName("Šifra")]
        [Required()]
        public String Password { get; set; }
        [DisplayName("Rola")]
        public String RoleName { get; set; }
        [DisplayName("Rola")]
        [Required()]
        public RoleEnum Role { get; set; }

        public UserModel()
        {

        }

        public UserModel(Guid id)
        {
            var user = UserLogic.GetUserByGuid(id);
            this.UserGuid = id;
            this.Email = user.Email;
            this.Password = user.Password;
        }

        public UserModel(int id)
        {
            var user = UserLogic.GetUserById(id);
            this.UserGuid = user.UserGuid;
            this.RoleName = user.RoleName;
            this.Email = user.Email;
        }

        public UserModel(string id)
        {
            try
            {
                Guid userGuid = new Guid(id);
                var user = UserLogic.GetUserByGuid(userGuid);
                this.UserGuid = userGuid;
                this.RoleName = user.RoleName;
                this.Email = user.Email;
            } catch (Exception e)
            {
                throw e;
            }
        }
             
        public static IEnumerable<UserModel> GetUsers()
        {
            return UserLogic.GetUsers().Select(user => new UserModel
            {
                UserGuid = user.UserGuid,
                Email = user.Email,
                RoleName = user.RoleName,
            });
        }

        public static IEnumerable<UserModel> GetUsersWaiting()
        {
            return UserLogic.GetUsersWaiting().Select(user => new UserModel
            {
                UserGuid = user.UserGuid,
                Email = user.Email,
                RoleName = user.RoleName,
            });
        }
    }
}