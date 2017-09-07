using Fakturiska.Business.DTOs;
using Fakturiska.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Fakturiska.Business.Logic
{
    public class UserLogic
    {

        public static UserDTO AuthorizeUser(string email, string password)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var user = dc.Users.Where(u => u.Email == email && u.Password == password && u.DeleteDate == null).ToList();
                if (user.Any()) {
                    return new UserDTO
                    {
                        Email = user.First().Email,
                        RoleName = user.First().Role.Description,
                        UserId = user.First().UserId
                    };
                } 
                return null;
            }
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

        public static void CreateUserWithoutPassword(UserDTO user)
        {
            User u = new User()
            {
                UserUId = user.UserGuid,
                Email = user.Email,
                RoleId = user.RoleId
            };

            using (var dc = new FakturiskaDBEntities())
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

                string body = "Click on this link to set your password: http://localhost:54276/Account/SetPassword/?id=" + user.UserGuid;
                SendMail(body, user.Email);
            }
        }

        public static void SetPassword(UserDTO user)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var u = GetUserById(user.UserGuid, dc);
                if (u != null && u.Password == null)
                {
                    u.Password = user.Password;
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

        public static UserDTO GetUserByGuid(Guid userGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var user = dc.Users.Where(u => u.UserUId == userGuid).FirstOrDefault();
                return new UserDTO
                {
                    UserGuid = user.UserUId,
                    RoleName = user.Role.Description,
                    Email = user.Email,
                    Password = user.Password
                };
            }
        }

        public static UserDTO GetUserById(int userId)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var user = dc.Users.Where(u => u.UserId == userId).FirstOrDefault();
                return new UserDTO
                {
                    UserGuid = user.UserUId,
                    RoleName = user.Role.Description,
                    Email = user.Email,
                };
            }
        }

        public static IEnumerable<UserDTO> GetAllUsers()
        {
            List<UserDTO> usersDTO = new List<UserDTO>();
            using (var dc = new FakturiskaDBEntities())
            {
                List<User> users = dc.Users.Where(user => user.DeleteDate == null && user.Password != null).ToList();
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

        public static void SendMail(string body, string to)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.mail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("pavlovicluka.99@mail.com", "proba123"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };
            MailMessage mail = new MailMessage
            {
                From = new MailAddress("pavlovicluka.99@mail.com", "Fakturiska"),
                Body = body
            };
            mail.To.Add(new MailAddress(to));

            smtpClient.Send(mail);
        }

        private static User GetUserById(Guid userGuid, FakturiskaDBEntities dc)
        {
            return dc.Users.Where(u => u.UserUId == userGuid && u.DeleteDate == null).FirstOrDefault();
        }
    }
}
