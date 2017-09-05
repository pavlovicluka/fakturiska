using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Business.DTOs;
using Fakturiska.Models;

namespace Fakturiska.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        public ActionResult Companies()
        {
            return View(CompanyModel.GetAllCompanies());
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
    }
}