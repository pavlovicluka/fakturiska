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
            InvoiceModel i = new InvoiceModel(invoice);

            int? receiverId = invoice.ReceiverId;
            CompanyModel comReceiver = null;
            if (receiverId != null)
            {
                var companyReceiver = CompanyLogic.GetCompanyById((int)receiverId);
                comReceiver = new CompanyModel(companyReceiver);
            }

            int? payerId = invoice.PayerId;
            CompanyModel comPayer = null;
            if (payerId != null)
            {
                var companyPayer = CompanyLogic.GetCompanyById((int)payerId);
                comPayer = new CompanyModel(companyPayer);
            }

            this.Invoice = i;
            this.CompanyReceiver = comReceiver;
            this.CompanyPayer = comPayer;
        }
    }
}