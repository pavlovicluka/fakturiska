using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Business.DTOs;
using Fakturiska.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Web;
using System.Collections.Generic;
using System.Net;
using System.Web.ModelBinding;

namespace Fakturiska.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        public ActionResult Invoices()
        {
            return View();
        }

        public ActionResult TableInvoices()
        {
            return PartialView("_TableInvoices", InvoiceModel.GetInvoices());
        }

        public ActionResult TableArchivedInvoices()
        {
            return PartialView("_TableArchivedInvoices", InvoiceModel.GetArchivedInvoices());
        }

        [HttpGet]
        public ActionResult CreateInvoice()
        {
            return PartialView("_CreateEditInvoice");
        }

        [HttpPost]
        public ActionResult CreateInvoice(InvoiceCompaniesModel model)
        {
            var identity = (ClaimsIdentity)User.Identity;
            InvoiceModel invoice = model.Invoice;
            CompanyModel companyReceiver = model.CompanyReceiver;
            CompanyModel companyPayer = model.CompanyPayer;

            if (invoice.InvoiceGuid != Guid.Empty && ModelState.ContainsKey("Invoice.File"))
                ModelState["Invoice.File"].Errors.Clear();

            if (ModelState.IsValid)
            {
                int? receiverId = null;
                if (companyReceiver != null)
                {
                    if(companyReceiver.CompanyGuid == Guid.Empty)
                    {
                        receiverId = CompanyLogic.CreateCompany(CompanyModel.MapModelToDTO(companyReceiver));
                    }
                    else
                    {
                        receiverId = CompanyLogic.EditCompany(CompanyModel.MapModelToDTO(companyReceiver));
                    }
                }

                int? payerId = null;
                if (companyPayer != null)
                {
                    if (companyPayer.CompanyGuid == Guid.Empty)
                    {
                        payerId = CompanyLogic.CreateCompany(CompanyModel.MapModelToDTO(companyPayer));
                    }
                    else
                    {
                        payerId = CompanyLogic.EditCompany(CompanyModel.MapModelToDTO(companyPayer));
                    }
                }

                if (invoice.InvoiceGuid == Guid.Empty)
                {
                    string filePath = InvoiceLogic.ConvertAndSaveFile(invoice.File);

                    InvoiceLogic.CreateInvoice(InvoiceModel.MapModelToDTO(invoice, int.Parse(identity.GetUserId()), receiverId, payerId, filePath));
                }
                else
                {
                    InvoiceLogic.EditInvoice(InvoiceModel.MapModelToDTO(invoice, null, receiverId, payerId, null));
                }

                if (invoice.Archive == null)
                {
                    return PartialView("_TableInvoices", InvoiceModel.GetInvoices());
                }
                return PartialView("_TableArchivedInvoices", InvoiceModel.GetArchivedInvoices());
            }
            return PartialView("_CreateEditInvoice", model);
        }

        [HttpPost]
        public ActionResult EditInvoice(Guid id)
        {
            return PartialView("_CreateEditInvoice", new InvoiceCompaniesModel(id));
        }

        [HttpPost]
        public ActionResult DeleteInvoice(Guid id)
        {
            InvoiceLogic.DeleteInvoice(id);
            return Json("Succeed");
        }

        [HttpPost]
        public ActionResult ArchiveInvoice(Guid id)
        {
            InvoiceLogic.ArchiveInvoice(id);
            return PartialView("_TableArchivedInvoices", InvoiceModel.GetArchivedInvoices());
        }

        public void PrintInvoice(String filePath)
        {
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(filePath);
            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.BinaryWrite(FileBuffer);
            }
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string filePath = InvoiceLogic.ConvertAndSaveFile(file);
            InvoiceLogic.CreateInvoice(new InvoiceDTO
            {
                InvoiceGuid = Guid.NewGuid(),
                UserId = int.Parse(identity.GetUserId()),
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