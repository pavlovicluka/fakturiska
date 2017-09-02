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
    }
}