using System;
using System.Web.Mvc;
using Fakturiska.Business.Logic;
using Fakturiska.Business.DTOs;
using Fakturiska.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Web;
using System.Collections.Generic;

namespace Fakturiska.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        public ActionResult Invoices()
        {
            return View(InvoiceModel.GetAllInvoices());
        }

        public ActionResult CreateInvoice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateInvoice(InvoiceCompaniesModel model)
        {
            var identity = (ClaimsIdentity)User.Identity;

            InvoiceModel invoice = model.Invoice;
            CompanyModel companyReceiver = model.CompanyReceiver;
            CompanyModel companyPayer = model.CompanyPayer;

            string filePath = InvoiceLogic.SaveFile(invoice.File);
            int receiverId = CompanyLogic.CreateCompany(new CompanyDTO
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
                UserId = int.Parse(identity.GetUserId()),
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

        public ActionResult DeleteInvoice(Guid id)
        {
            InvoiceLogic.DeleteInvoice(id);
            return RedirectToAction("Invoices");
        }

        public ActionResult ArchiveInvoice(Guid id)
        {
            InvoiceLogic.ArchiveInvoice(id);
            return RedirectToAction("Invoices");
        }

        [HttpGet]
        public ActionResult PrintInvoice(String filePath)
        {
            InvoiceLogic.PrintInvoice(filePath);
            return RedirectToAction("Invoices");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string filePath = InvoiceLogic.SaveFile(file);
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