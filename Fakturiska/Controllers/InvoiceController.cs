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

            int? receiverId = null;
            if (companyReceiver != null)
            {
                receiverId = CompanyLogic.CreateCompany(new CompanyDTO
                {
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
            }

            int? payerId = null;
            if (companyPayer != null)
            {
                payerId = CompanyLogic.CreateCompany(new CompanyDTO
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
            }

            if (invoice.InvoiceGuid == Guid.Empty)
            {
                string filePath = InvoiceLogic.ConvertAndSaveFile(invoice.File);

                InvoiceLogic.CreateInvoice(new InvoiceDTO
                {
                    InvoiceGuid = Guid.NewGuid(),
                    UserId = int.Parse(identity.GetUserId()),
                    Date = invoice.Date,
                    InvoiceEstimate = Convert.ToInt32(invoice.InvoiceEstimateChecked),
                    InvoiceTotal = Convert.ToInt32(invoice.InvoiceTotalChecked),
                    Incoming = Convert.ToInt32(invoice.IncomingChecked),
                    Paid = Convert.ToInt32(invoice.PaidChecked),
                    Risk = Convert.ToInt32(invoice.RiskChecked),
                    PriorityId = (int)invoice.Priority + 1,
                    Sum = invoice.Sum,
                    ReceiverId = receiverId,
                    PayerId = payerId,
                    FilePath = filePath,
                });
            }
            else
            {
                InvoiceLogic.EditInvoice(new InvoiceDTO
                {
                    InvoiceGuid = invoice.InvoiceGuid,
                    Date = invoice.Date,
                    InvoiceEstimate = Convert.ToInt32(invoice.InvoiceEstimateChecked),
                    InvoiceTotal = Convert.ToInt32(invoice.InvoiceTotalChecked),
                    Incoming = Convert.ToInt32(invoice.IncomingChecked),
                    Paid = Convert.ToInt32(invoice.PaidChecked),
                    Risk = Convert.ToInt32(invoice.RiskChecked),
                    Sum = invoice.Sum,
                    PriorityId = (int)invoice.Priority + 1,
                    ReceiverId = receiverId,
                    PayerId = payerId,
                });
            }

            if(invoice.Archive == null)
            {
                return PartialView("_TableInvoices", InvoiceModel.GetInvoices());
            }
            return PartialView("_TableArchivedInvoices", InvoiceModel.GetArchivedInvoices());
        }

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
            return Json("Succeed");
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
    }
}