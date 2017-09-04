using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Business.DTOs;
using Fakturiska.Models;
using System.Collections.Generic;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Fakturiska.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (Session["userId"] != null) return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            string role = UserLogic.AuthorizeUser(model.Email, model.Password);
            if (role != null)
            {
                var ident = new ClaimsIdentity(
                 new[] {
                  new Claim(ClaimTypes.NameIdentifier, model.Email),
                  new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                  new Claim(ClaimTypes.Name, model.Email),
                  new Claim(ClaimTypes.Role, role),
                }, DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        public ActionResult SignUp()
        {
            return View(new UserModel());
        }

        [HttpPost]
        public ActionResult SignUp(UserModel model)
        {

            UserLogic.CreateUser(new UserDTO
            {
                UserGuid = Guid.NewGuid(),
                Email = model.Email,
                Password = model.Password,
                RoleId = (int)model.Role,
            });
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult EditUser(string value, string pk)
        {
            UserLogic.EditUser(new UserDTO
            {
                UserGuid = new Guid(pk),
                Email = value,
            });
            return RedirectToAction("Users");
        }

        public ActionResult DeleteUser(Guid id)
        {
            UserLogic.DeleteUser(id);
            return RedirectToAction("Users");
        }

        public ActionResult Users()
        {
            return View(UserModel.GetAllUsers());
        }

        public ActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCompany(CompanyModel company)
        {

            CompanyLogic.CreateCompany(new CompanyDTO
            {
                CompanyGuid = Guid.NewGuid(),
                Name = company.Name,
                PhoneNumber = company.PhoneNumber,
                FaxNumber = company.FaxNumber,
                Address = company.Address,
                Website = company.Website,
                Email = company.Email,
                PersonalNumber = company.PersonalNumber,
                PIB = company.PIB,
                MIB = company.MIB,
                AccountNumber = company.AccountNumber,
                BankCode = company.BankCode
            });

            return RedirectToAction("Companies");
        }

        public ActionResult EditCompany(Guid id)
        {
            return View(new CompanyModel(id));
        }

        [HttpPost]
        public ActionResult EditCompany(CompanyModel company)
        {
            CompanyLogic.EditCompany(new CompanyDTO
            {
                CompanyGuid = company.CompanyGuid,
                Name = company.Name,
                PhoneNumber = company.PhoneNumber,
                FaxNumber = company.FaxNumber,
                Address = company.Address,
                Website = company.Website,
                Email = company.Email,
                PersonalNumber = company.PersonalNumber,
                PIB = company.PIB,
                MIB = company.MIB,
                AccountNumber = company.AccountNumber,
                BankCode = company.BankCode
            });
            return RedirectToAction("Companies");
        }

        public ActionResult DeleteCompany(Guid id)
        {
            CompanyLogic.DeleteCompany(id);
            return RedirectToAction("Companies");
        }

        public ActionResult Companies()
        {
            return View(CompanyModel.GetAllCompanies());
        }

        public ActionResult CreateInvoice()
        {
            if (Session["userId"] == null) return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        public ActionResult CreateInvoice(InvoiceCompaniesModel model)
        {
            InvoiceModel invoice = model.Invoice;
            CompanyModel companyReceiver = model.CompanyReceiver;
            CompanyModel companyPayer = model.CompanyPayer;

            string filePath = InvoiceLogic.SaveFile(invoice.File);
            int receiverId = CompanyLogic.CreateCompany(new CompanyDTO {
                CompanyGuid = Guid.NewGuid(),
                Name = companyReceiver.Name,
                PhoneNumber = companyReceiver.PhoneNumber,
                FaxNumber = companyReceiver.FaxNumber,
                Address = companyReceiver.Address,
                Website = companyReceiver.Website,
                Email = companyReceiver.Email,
                PersonalNumber = companyReceiver.PersonalNumber,
                PIB = companyReceiver.PIB,
                MIB = companyReceiver.MIB,
                AccountNumber = companyReceiver.AccountNumber,
                BankCode = companyReceiver.BankCode
            });
            int payerId = CompanyLogic.CreateCompany(new CompanyDTO
            {
                CompanyGuid = Guid.NewGuid(),
                Name = companyPayer.Name,
                PhoneNumber = companyPayer.PhoneNumber,
                FaxNumber = companyPayer.FaxNumber,
                Address = companyPayer.Address,
                Website = companyPayer.Website,
                Email = companyPayer.Email,
                PersonalNumber = companyPayer.PersonalNumber,
                PIB = companyPayer.PIB,
                MIB = companyPayer.MIB,
                AccountNumber = companyPayer.AccountNumber,
                BankCode = companyPayer.BankCode
            });

            InvoiceLogic.CreateInvoice(new InvoiceDTO
            {
                InvoiceGuid = Guid.NewGuid(),
                UserId = int.Parse(Session["userId"].ToString()),
                Date = invoice.Date,
                Sum = invoice.Sum,
                InvoiceEstimate = Convert.ToInt32(invoice.InvoiceEstimate),
                InvoiceTotal = Convert.ToInt32(invoice.InvoiceTotal),
                Incoming = Convert.ToInt32(invoice.Incoming),
                Paid = Convert.ToInt32(invoice.Paid),
                Risk = Convert.ToInt32(invoice.Risk),
                PriorityId = (int)invoice.Priority,
                ReceiverId = receiverId,
                PayerId = payerId,
                FilePath = filePath,
            });
            return RedirectToAction("Invoices");
        }

        public ActionResult ArchiveInvoice(Guid id)
        {
            InvoiceLogic.ArchiveInvoice(id);
            return RedirectToAction("Invoices");
        }

        public ActionResult DeleteInvoice(Guid id)
        {
            InvoiceLogic.DeleteInvoice(id);
            return RedirectToAction("Invoices");
        }

        [HttpGet]
        public ActionResult PrintInvoice(String filePath)
        {
            InvoiceLogic.PrintInvoice(filePath);
            return RedirectToAction("Invoices");
        }

        [AuthorizeUser(Roles = "Admin")]
        public ActionResult Invoices()
        {
            return View(InvoiceModel.GetAllInvoices());
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            string filePath = InvoiceLogic.SaveFile(file);
            InvoiceLogic.CreateInvoice(new InvoiceDTO
            {
                InvoiceGuid = Guid.NewGuid(),
                UserId = int.Parse(Session["userId"].ToString()),
                FilePath = filePath
            });
            return RedirectToAction("Invoices");
        }

        [HttpPost]
        public JsonResult Autocomplete(string prefix)
        {
            IEnumerable<CompanyDTO> allCompanies = CompanyLogic.GetAllCompaniesAutocomplete(prefix);
            return Json(allCompanies, JsonRequestBehavior.AllowGet);
        }
    }
}