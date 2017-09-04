using Fakturiska.Business.DTOs;
using Fakturiska.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakturiska.Business.Logic
{
    public class UserLogic
    {

        public static string AuthorizeUser(string email, string password)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var user = dc.Users.Where(u => u.Email == email && u.Password == password && u.DeleteDate == null).ToList();
                if (user.Any()) return user.First().Role.Description;
                return "";
            }
        }

        public static string[] GetUserRights(string v)
        {
            string[] a = new string[] { "Admin", "User"};
            return a;
        }

        public static void CreateUser(UserDTO user)
        {
            User u = new User()
            {
                UserUId = user.UserGuid,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId
            };

            using(var dc = new FakturiskaDBEntities())
            {
                dc.Users.Add(u);

                try
                {
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static UserDTO GetUserById(Guid userGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var user = dc.Users.Where(u => u.UserUId == userGuid).FirstOrDefault();
                return new UserDTO
                {
                    UserGuid = user.UserUId,
                    Email = user.Email,
                    Password = user.Password
                };
            }
        }

        public static void EditUser(UserDTO user)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var u = GetUserById(user.UserGuid, dc);
                if (u != null)
                {
                    u.Email = user.Email;
                }
                try
                {
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static void DeleteUser(Guid userGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var user = GetUserById(userGuid, dc);
                if (user != null)
                {
                    user.DeleteDate = DateTime.Now;
                }
                try
                {
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static IEnumerable<UserDTO> GetAllUsers()
        {
            List<UserDTO> usersDTO = new List<UserDTO>();
            using (var dc = new FakturiskaDBEntities())
            {
                List<User> users = dc.Users.Where(user => user.DeleteDate == null).ToList();
                foreach(var user in users)
                {
                    usersDTO.Add(new UserDTO
                    {
                        UserGuid = user.UserUId,
                        Email = user.Email,
                        RoleId = user.RoleId,
                    });
                }
            }
            return usersDTO;
        }

        private static User GetUserById(Guid userGuid, FakturiskaDBEntities dc)
        {
            return dc.Users.Where(u => u.UserUId == userGuid && u.DeleteDate == null).FirstOrDefault();
        }
    }
}
