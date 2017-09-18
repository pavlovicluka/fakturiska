using Fakturiska.Business.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fakturiska.Models
{
    public class UserModelWithoutPassword
    {
        public Guid UserGuid { get; set; }
        [DisplayName("Email")]
        [Required()]
        public String Email { get; set; }
        [DisplayName("Rola")]
        [Required()]
        public RoleEnum Role { get; set; }

        public UserModelWithoutPassword()
        {

        }
    }
}