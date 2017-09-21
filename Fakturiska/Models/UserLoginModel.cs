using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fakturiska.Models
{
    public class UserLoginModel
    {
        [DisplayName("Email")]
        [Required()]
        public String Email { get; set; }
        [DisplayName("Šifra")]
        [Required()]
        public String Password { get; set; }

        public UserLoginModel()
        {

        }
    }
}