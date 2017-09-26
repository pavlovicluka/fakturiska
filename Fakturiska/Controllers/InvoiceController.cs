using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Business.DTOs;
using Fakturiska.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Web;
using System.Collections.Generic;
using System.IO;
using MimeSharp;

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
            return PartialView("_CreateEditInvoice", new InvoiceCompaniesModel());
        }

        [HttpPost]
        public ActionResult CreateInvoice(InvoiceCompaniesModel invoiceCompaniesModel)
        {
            var identity = (ClaimsIdentity)User.Identity;
            InvoiceModel invoice = invoiceCompaniesModel.Invoice;
            CompanyModel companyReceiver = invoiceCompaniesModel.CompanyReceiver;
            CompanyModel companyPayer = invoiceCompaniesModel.CompanyPayer;

            if (ModelState.IsValid)
            {
                Dictionary<string, int?> response = InvoiceLogic.CreateEditInvoice(InvoiceModel.MapModelToDTO(invoice, int.Parse(identity.GetUserId())), 
                                                                                  CompanyModel.MapModelToDTO(companyReceiver), 
                                                                                  CompanyModel.MapModelToDTO(companyPayer));

                foreach (KeyValuePair<string, int?> entry in response)
                {
                    switch (entry.Key)
                    {
                        case "companyReceiver":
                            if(entry.Value == null || entry.Value == 0)
                            {
                                ModelState.AddModelError("CompanyReceiver", "Forma nije popunjena");
                            }
                            break;
                        case "companyReceiverNameExists":
                            ModelState.AddModelError("CompanyReceiver.Name", "Ovo ime već postoji");
                            break;
                        case "companyReceiverPersonalNumberExists":
                            ModelState.AddModelError("CompanyReceiver.PersonalNumber", "Ovaj lični broj već postoji");
                            break;
                        case "companyReceiverPIBExists":
                            ModelState.AddModelError("CompanyReceiver.PIB", "Ovaj PIB već postoji");
                            break;
                        case "companyReceiverCannotEdit":
                            ModelState.AddModelError("CompanyReceiver", "Ne možete menjati podatke pravnog lica");
                            break;
                        case "companyPayer":
                            if (entry.Value == null || entry.Value == 0)
                            {
                                ModelState.AddModelError("CompanyPayer", "Forma nije popunjena");
                            }
                            break;
                        case "companyPayerNameExists":
                            ModelState.AddModelError("CompanyPayer.Name", "Ovo ime već postoji");
                            break;
                        case "companyPayerrPersonalNumberExists":
                            ModelState.AddModelError("CompanyPayer.PersonalNumber", "Ovaj lični broj već postoji");
                            break;
                        case "companyPayerPIBExists":
                            ModelState.AddModelError("CompanyPayer.PIB", "Ovaj PIB već postoji");
                            break;
                        case "companyPayerCannotEdit":
                            ModelState.AddModelError("CompanyReceiver", "Ne možete menjati podatke pravnog lica");
                            break;
                        case "FileProblem":
                            ModelState.AddModelError("Invoice", "Greška pri dodavanju fajla");
                            break;
                        case "success":
                            if (invoice.Archive == null)
                            {
                                return PartialView("_TableInvoices", InvoiceModel.GetInvoices());
                            }
                            return PartialView("_TableArchivedInvoices", InvoiceModel.GetArchivedInvoices());
                    }
                }

            }
            return PartialView("_CreateEditInvoice", invoiceCompaniesModel);
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

        public void PrintInvoice(Guid guid)
        {
            string filePath = InvoiceLogic.GetFilePathByGuid(guid);
            FileInfo fileInfo = new FileInfo(filePath);
            var mime = new Mime();
            string mimeType = mime.Lookup(filePath);

            Response.AppendHeader("Connection", "keep-alive");
            Response.AppendHeader("Content-Type", mimeType);
            Response.AppendHeader("Expires", "0");
            Response.AppendHeader("Cache-Control", "max-age=864000");
            Response.AppendHeader("Content-Length:", fileInfo.Length.ToString());
            Response.Flush();

            Response.TransmitFile(filePath);
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string filePath = InvoiceLogic.SaveFile(file);
            if(filePath != "")
            {
                InvoiceLogic.CreateInvoice(new InvoiceDTO
                {
                    UserId = int.Parse(identity.GetUserId()),
                    InvoiceEstimate = false,
                    InvoiceTotal = false,
                    Incoming = false,
                    Paid = false,
                    Risk = false,
                    FilePath = filePath
                });
            }
            return Json("success");
        }

        [HttpPost]
        public JsonResult Autocomplete(string prefix, int fieldCase)
        {
            IEnumerable<CompanyDTO> allCompanies = CompanyLogic.GetAllCompaniesAutocomplete(prefix, fieldCase);
            return Json(allCompanies, JsonRequestBehavior.AllowGet);
        }
    }
}