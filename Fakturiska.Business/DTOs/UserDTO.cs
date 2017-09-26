using Fakturiska.Business.Enumerations;
using Fakturiska.Database;
using System;

namespace Fakturiska.Business.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public RoleEnum Role { get; set; }
        public DateTime DeleteDate { get; set; }

        public UserDTO()
        {

        }

        public UserDTO(User user)
        {
            UserId = user.UserId;
            UserGuid = user.UserUId;
            Email = user.Email;
            Password = user.Password;
            Role = (RoleEnum)(user.RoleId - 1);
        }
    }
}
