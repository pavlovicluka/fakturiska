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
            return PartialView("_CreateEditInvoice");
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
                int? receiverId = null;
                if (companyReceiver != null)
                {
                    if(companyReceiver.CompanyGuid == null || companyReceiver.CompanyGuid == Guid.Empty)
                    {
                        List<int> response = CompanyLogic.CreateCompany(CompanyModel.MapModelToDTO(companyReceiver));

                        foreach (var res in response)
                        {
                            receiverId = res;
                            switch (res)
                            {
                                case -1:
                                    ModelState.AddModelError("CompanyReceiver.Name", "Ovo ime vec postoji");
                                    break;
                                case -2:
                                    ModelState.AddModelError("CompanyReceiver.PersonalNumber", "Ovaj licni broj vec postoji");
                                    break;
                                case -3:
                                    ModelState.AddModelError("CompanyReceiver.PIB", "Ovaj PIB vec postoji");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        receiverId = CompanyLogic.CheckCompany(CompanyModel.MapModelToDTO(companyReceiver));
                        if(receiverId <= 0)
                        {
                            switch (receiverId)
                            {
                                case 0:
                                    ModelState.AddModelError(string.Empty, "Greska!");
                                    break;
                                case -1:
                                    ModelState.AddModelError(string.Empty, "Ne mozete menjati ime kompanije, njen licni broj ni PIB");
                                    break;
                            }
                        }
                    }
                }

                int? payerId = null;
                if (companyPayer != null)
                {
                    if (companyPayer.CompanyGuid == null || companyPayer.CompanyGuid == Guid.Empty)
                    {
                        List<int> response = CompanyLogic.CreateCompany(CompanyModel.MapModelToDTO(companyPayer));

                        foreach (var res in response)
                        {
                            switch (res)
                            {
                                case -1:
                                    ModelState.AddModelError("CompanyPayer.Name", "Ovo ime vec postoji");
                                    break;
                                case -2:
                                    ModelState.AddModelError("CompanyPayer.PersonalNumber", "Ovaj licni broj vec postoji");
                                    break;
                                case -3:
                                    ModelState.AddModelError("CompanyPayer.PIB", "Ovaj PIB vec postoji");
                                    break;
                                default:
                                    payerId = res;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        payerId = CompanyLogic.CheckCompany(CompanyModel.MapModelToDTO(companyPayer));
                        if (payerId <= 0)
                        {
                            switch (payerId)
                            {
                                case 0:
                                    ModelState.AddModelError(string.Empty, "Greska!");
                                    break;
                                case -1:
                                    ModelState.AddModelError(string.Empty, "Ne mozete menjati ime kompanije, njen licni broj ni PIB");
                                    break;
                            }
                        }
                    }
                }

                if(receiverId <= 0 || payerId <= 0)
                {
                    return PartialView("_CreateEditInvoice", invoiceCompaniesModel);
                }

                if (invoice.InvoiceGuid == null || invoice.InvoiceGuid == Guid.Empty)
                {
                    string filePath = InvoiceLogic.SaveFile(invoice.File);
                    if(filePath != "")
                    {
                        InvoiceLogic.CreateInvoice(InvoiceModel.MapModelToDTO(invoice, int.Parse(identity.GetUserId()), receiverId, payerId, filePath));
                    }
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