using Fakturiska.Business.Enumerations;
using Fakturiska.Business.Logic;
using Foolproof;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fakturiska.Models
{
    public class UserChangePasswordModel
    {
        public Guid UserGuid { get; set; }

        [DisplayName("Email")]
        public String Email { get; set; }

        [DisplayName("Stara Šifra")]
        [Required()]
        public String OldPassword { get; set; }

        [DisplayName("Šifra")]
        [Required()]
        [RegularExpression("(?=^.{8,15}$)((?!.*\\s)(?=.*[A-Z])(?=.*[a-z])(?=(.*\\d){1,}))((?!.*[\",;&| '])|(?=(.*\\W){1,}))(?!.*[\",;&|'])^.*$",
                            ErrorMessage = "Šifra mora imati najmanje 8 karaktera, najmanje jedno veliko slovo i jedan broj")]
        public String Password { get; set; }

        [DisplayName("Potvrdite Šifru")]
        [Required()]
        [EqualTo("Password", ErrorMessage = "Šifre se ne poklapaju")]
        public String PasswordConfirm { get; set; }

        [DisplayName("Rola")]
        public RoleEnum Role { get; set; }

        public UserChangePasswordModel()
        {

        }

        public UserChangePasswordModel(int id)
        {
            var user = UserLogic.GetUserById(id);
            this.UserGuid = user.UserGuid;
            this.Role = user.Role;
            this.Email = user.Email;
        }
    }
}