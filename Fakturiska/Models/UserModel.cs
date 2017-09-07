using Fakturiska.Business.Enumerations;
using Fakturiska.Business.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakturiska.Models
{
    public class UserModel
    {
        public Guid UserGuid { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String RoleName { get; set; }
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
             
        public static IEnumerable<UserModel> GetAllUsers()
        {
            return UserLogic.GetAllUsers().Select(user => new UserModel
            {
                UserGuid = user.UserGuid,
                Email = user.Email,
                RoleName = ((RoleEnum) user.RoleId).ToString(),
            });
        }
    }
}