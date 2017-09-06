using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakturiska.Business.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime DeleteDate { get; set; }
    }
}
