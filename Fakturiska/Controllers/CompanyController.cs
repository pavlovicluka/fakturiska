using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Business.DTOs;
using Fakturiska.Models;
using System.Web.ModelBinding;

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
            return PartialView("_CreateEditCompany");
        }

        [HttpPost]
        public ActionResult CreateCompany(CompanyModel company)
        {
            //if (ModelState.IsValid)
            //{
                if (company.CompanyGuid == null || company.CompanyGuid == Guid.Empty)
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
                }
                else
                {
                    CompanyLogic.EditCompany(new CompanyDTO
                    {
                        CompanyGuid = (Guid)company.CompanyGuid,
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
                }
                return RedirectToAction("Companies");
           // }
            return PartialView("_CreateEditCompany", company);
        }

        public ActionResult EditCompany(Guid id)
        {
            return PartialView("_CreateEditCompany", new CompanyModel(id));
        }

        [HttpPost]
        public ActionResult DeleteCompany(Guid id)
        {
            CompanyLogic.DeleteCompany(id);
            return RedirectToAction("Companies");
        }
    }
}