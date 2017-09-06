using Fakturiska.Business.DTOs;
using Fakturiska.Business.Enumerations;
using Fakturiska.Business.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakturiska.Models
{
    public class InvoiceCompaniesModel
    {
        public InvoiceModel Invoice { get; set; }
        public CompanyModel CompanyReceiver { get; set; }
        public CompanyModel CompanyPayer { get; set; }


        public InvoiceCompaniesModel()
        {

        }

        public InvoiceCompaniesModel(Guid id)
        {
            var invoice = InvoiceLogic.GetInvoiceByGuid(id);
            InvoiceModel i = new InvoiceModel();
            i.InvoiceGuid = id;
            i.Date = invoice.Date;
            i.InvoiceEstimate = Convert.ToBoolean(invoice.InvoiceEstimate);
            i.InvoiceTotal = Convert.ToBoolean(invoice.InvoiceTotal);
            i.Incoming = Convert.ToBoolean(invoice.Incoming);
            i.Paid = Convert.ToBoolean(invoice.Paid);
            i.Risk = Convert.ToBoolean(invoice.Risk);
            i.Sum = invoice.Sum;
            if(invoice.PriorityId != null)
                i.Priority = (PriorityEnum)invoice.PriorityId;

            int? receiverId = invoice.ReceiverId;
            CompanyModel comReceiver = new CompanyModel();
            if (receiverId != null)
            {
                var companyReceiver = CompanyLogic.GetCompanyById((int)receiverId);
                comReceiver.CompanyGuid = companyReceiver.CompanyGuid;
                comReceiver.Name = companyReceiver.Name;
                comReceiver.PhoneNumber = companyReceiver.PhoneNumber;
                comReceiver.FaxNumber = companyReceiver.FaxNumber;
                comReceiver.Address = companyReceiver.Address;
                comReceiver.Website = companyReceiver.Website;
                comReceiver.Email = companyReceiver.Email;
                comReceiver.PersonalNumber = companyReceiver.PersonalNumber;
                comReceiver.PIB = companyReceiver.PIB;
                comReceiver.MIB = companyReceiver.MIB;
                comReceiver.AccountNumber = companyReceiver.AccountNumber;
                comReceiver.BankCode = companyReceiver.BankCode;
            }

            int? payerId = invoice.PayerId;
            CompanyModel comPayer = new CompanyModel();
            if (payerId != null)
            {
                var companyPayer = CompanyLogic.GetCompanyById((int)payerId);
                comPayer.CompanyGuid = companyPayer.CompanyGuid;
                comPayer.Name = companyPayer.Name;
                comPayer.PhoneNumber = companyPayer.PhoneNumber;
                comPayer.FaxNumber = companyPayer.FaxNumber;
                comPayer.Address = companyPayer.Address;
                comPayer.Website = companyPayer.Website;
                comPayer.Email = companyPayer.Email;
                comPayer.PersonalNumber = companyPayer.PersonalNumber;
                comPayer.PIB = companyPayer.PIB;
                comPayer.MIB = companyPayer.MIB;
                comPayer.AccountNumber = companyPayer.AccountNumber;
                comPayer.BankCode = companyPayer.BankCode;
            }

            this.Invoice = i;
            this.CompanyReceiver = comReceiver;
            this.CompanyPayer = comPayer;
        }
    }
}