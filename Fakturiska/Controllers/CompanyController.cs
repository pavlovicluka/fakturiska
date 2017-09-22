using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Models;
using Fakturiska.Business.DTOs;
using System.Collections.Generic;

namespace Fakturiska.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        public ActionResult Companies()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateCompany()
        {
            return PartialView("_CreateEditCompany");
        }

        [HttpPost]
        public ActionResult CreateCompany(CompanyModel company)
        {
            if (ModelState.IsValid)
            {
                List<int> response = null;
                if (company.CompanyGuid == null || company.CompanyGuid == Guid.Empty)
                {
                    response = CompanyLogic.CreateCompany(CompanyModel.MapModelToDTO(company));

                }
                else
                {
                    response = CompanyLogic.EditCompany(CompanyModel.MapModelToDTO(company));
                }

                foreach (var res in response)
                {
                    switch (res)
                    {
                        case -1:
                            ModelState.AddModelError("Name", "Ovo ime vec postoji");
                            break;
                        case -2:
                            ModelState.AddModelError("PersonalNumber", "Ovaj licni broj vec postoji");
                            break;
                        case -3:
                            ModelState.AddModelError("PIB", "Ovaj PIB vec postoji");
                            break;
                        default:
                            return Json("success");
                    }
                }
            }
            return PartialView("_CreateEditCompany", company);
        }

        [HttpPost]
        public ActionResult EditCompany(Guid id)
        {
            return PartialView("_CreateEditCompany", new CompanyModel(id));
        }

        [HttpPost]
        public ActionResult DeleteCompany(Guid id)
        {
            CompanyLogic.DeleteCompany(id);
            return Json("success");
        }

        public JsonResult ServerSideSearchAction(DataTableAjaxPostModel model)
        {
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchCompanies(model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                sEcho = model.draw,
                iTotalRecords = totalResultsCount,
                iTotalDisplayRecords = filteredResultsCount,
                aaData = res
            });
        }

        public IList<CompanyDTO> SearchCompanies(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            string sortDir = "asc";

            if (model.order != null)
            {
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower();
            }

            var result = CompanyLogic.GetCompanies(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                return new List<CompanyDTO>();
            }
            return result;
        }
    }
}