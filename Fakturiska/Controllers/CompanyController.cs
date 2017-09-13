using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Models;

namespace Fakturiska.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        public ActionResult Companies()
        {
            return View();
        }

        public ActionResult TableCompanies()
        {
            return PartialView("_TableCompanies", CompanyModel.GetAllCompanies());
        }

        public ActionResult CreateCompany()
        {
            return PartialView("_CreateEditCompany");
        }

        [HttpPost]
        public ActionResult CreateCompany(CompanyModel company)
        {
            if (ModelState.IsValid)
            {
                if (company.CompanyGuid == null || company.CompanyGuid == Guid.Empty)
                {
                    CompanyLogic.CreateCompany(CompanyModel.MapModelToDTO(company));
                }
                else
                {
                    CompanyLogic.CreateCompany(CompanyModel.MapModelToDTO(company));
                }
                return PartialView("_TableCompanies", CompanyModel.GetAllCompanies());
            }
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
            return Json("Succeed");
        }
    }
}