using Fakturiska.Business.DTOs;
using Fakturiska.Database;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Fakturiska.Business.Logic
{
    public class UserLogic
    {

        public static UserDTO AuthorizeUser(string email, string password)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var user = dc.Users.FirstOrDefault(u => u.Email == email && u.DeleteDate == null);
                if (user != null)
                {
                    if (PasswordStorage.VerifyPassword(password, user.Password))
                    {
                        return new UserDTO(user);
                    }
                }
                return null;
            }
        }

        public static void CreateUser(UserDTO user)
        {
            User u = new User()
            {
                UserUId = Guid.NewGuid(),
                Email = user.Email,
                Password = PasswordStorage.CreateHash(user.Password),
                RoleId = (int)user.Role + 1
            };

            using (var dc = new FakturiskaDBEntities())
            {
                dc.Users.Add(u);
                dc.SaveChanges();
            }
        }

        public static string CreateUserWithoutPassword(UserDTO user)
        {
            var us = GetUserIdByEmail(user.Email);
            if (us == null)
            {
                Guid newUserGuid = Guid.NewGuid();
                User u = new User()
                {
                    UserUId = newUserGuid,
                    Email = user.Email,
                    RoleId = (int)user.Role + 1
                };

                using (var dc = new FakturiskaDBEntities())
                {
                    dc.Users.Add(u);
                    dc.SaveChanges();

                    string body = "Kliknite na ovaj link kako biste podesili šifru: http://localhost:54278/Account/SetPassword/?id=" + newUserGuid;
                    SendMail(body, user.Email);
                }
                return "success";
            }
            return "exists";
        }

        public static void SetPassword(UserDTO user)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var u = GetUserByGuid(user.UserGuid, dc);
                if (u != null && u.Password == null)
                {
                    u.Password = PasswordStorage.CreateHash(user.Password);
                }
                dc.SaveChanges();
                var context = GlobalHost.ConnectionManager.GetHubContext<RealTime>();
                context.Clients.All.NewUser("Korisnik " + u.Email + " se prijavio prvi put na portal Fakturiška");
            }
        }

        public static string ChangePassword(UserDTO user)
        {
            string response = "";
            using (var dc = new FakturiskaDBEntities())
            {
                var u = GetUserByGuid(user.UserGuid, dc);
                if (u != null && u.Email == user.Email && PasswordStorage.VerifyPassword(user.OldPassword, u.Password))
                {
                    u.Password = PasswordStorage.CreateHash(user.Password);
                    response = "success";
                }
                else
                {
                    response = "wrongPassword";
                }
                dc.SaveChanges();
            }
            return response;
        }

        public static void EditUserEmail(UserDTO user)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var u = GetUserByGuid(user.UserGuid, dc);
                if (u != null)
                {
                    u.Email = user.Email;
                }
                dc.SaveChanges();
            }
        }

        public static void EditUserRole(UserDTO user)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var u = GetUserByGuid(user.UserGuid, dc);
                if (u != null)
                {
                    u.RoleId = (int)user.Role;
                }
                dc.SaveChanges();
            }
        }

        public static void DeleteUser(Guid userGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var user = GetUserByGuid(userGuid, dc);
                if (user != null)
                {
                    user.DeleteDate = DateTime.Now;
                }
                dc.SaveChanges();
            }
        }

        public static UserDTO GetUserByGuid(Guid userGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var user = dc.Users.FirstOrDefault(u => u.DeleteDate == null && u.UserUId == userGuid);
                return new UserDTO(user);
            }
        }

        public static UserDTO GetUserById(int userId)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var user = dc.Users.FirstOrDefault(u => u.DeleteDate == null && u.UserId == userId);
                return new UserDTO(user);
            }
        }

        public static int? GetUserIdByEmail(string email)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var user = dc.Users.FirstOrDefault(u => u.DeleteDate == null && u.Email == email);
                if (user != null)
                {
                    return user.UserId;
                }
                else
                {
                    return null;
                }
            }
        }

        public static IEnumerable<UserDTO> GetUsers()
        {
            List<UserDTO> usersDTOs = null;
            using (var dc = new FakturiskaDBEntities())
            {
                var users = dc.Users.Where(user => user.DeleteDate == null && user.Password != null).ToList();
                usersDTOs = users.Select(user => new UserDTO(user)).ToList();
            }
            return usersDTOs;
        }

        public static IEnumerable<UserDTO> GetUsersWaiting()
        {
            List<UserDTO> usersDTOs = null;
            using (var dc = new FakturiskaDBEntities())
            {
                var users = dc.Users.Where(user => user.DeleteDate == null && user.Password == null).ToList();
                usersDTOs = users.Select(user => new UserDTO(user)).ToList();
            }
            return usersDTOs;
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
                Subject = "Otvoren nalog na portalu Fakturiška",
                From = new MailAddress("pavlovicluka.99@mail.com", "Fakturiška"),
                Body = body
            };
            mail.To.Add(new MailAddress(to));

            smtpClient.Send(mail);
        }

        private static User GetUserByGuid(Guid userGuid, FakturiskaDBEntities dc)
        {
            return dc.Users.FirstOrDefault(u => u.UserUId == userGuid && u.DeleteDate == null);
        }
    }
}
