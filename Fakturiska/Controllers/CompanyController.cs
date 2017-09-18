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

        public ActionResult TableCompanies()
        {
            return PartialView("_TableCompanies", CompanyModel.GetAllCompanies());
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
                if (company.CompanyGuid == null || company.CompanyGuid == Guid.Empty)
                {
                    CompanyLogic.CreateCompany(CompanyModel.MapModelToDTO(company));
                }
                else
                {
                    CompanyLogic.EditCompany(CompanyModel.MapModelToDTO(company));
                }
                return PartialView("_TableCompanies", CompanyModel.GetAllCompanies());
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
            return Json("Succeed");
        }

        public JsonResult ServerSideSearchAction(DataTableAjaxPostModel model)
        {
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchCompanies(model, out filteredResultsCount, out totalResultsCount);

            /*var result = new List<CompanyDTO>(res.Count);
            int i = 0;
            foreach (var s in res)
            {
                result.Add(res[i]);
                i++;
            };*/

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