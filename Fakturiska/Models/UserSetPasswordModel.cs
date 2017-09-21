using Fakturiska.Business.DTOs;
using Fakturiska.Business.Enumerations;
using Fakturiska.Business.Logic;
using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fakturiska.Models
{
    public class UserSetPasswordModel
    {
        public Guid UserGuid { get; set; }
        [DisplayName("Email")]
        public String Email { get; set; }
        [DisplayName("Šifra")]
        [Required()]
        public String Password { get; set; }
        [DisplayName("Potvrdite Šifru")]
        [Required()]
        [EqualTo("Password", ErrorMessage = "Šifre se ne poklapaju")]
        public String PasswordConfirm { get; set; }
        [DisplayName("Rola")]
        public RoleEnum Role { get; set; }

        public UserSetPasswordModel()
        {

        }

        /*public UserModel(Guid id)
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
        }*/

        public UserSetPasswordModel(string id)
        {
            try
            {
                Guid userGuid = new Guid(id);
                var user = UserLogic.GetUserByGuid(userGuid);
                this.UserGuid = userGuid;
                this.Role = user.Role;
                this.Email = user.Email;
            } catch (Exception e)
            {
                throw e;
            }
        }
    }
}