using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakturiska.Business.DTOs
{
    public class CompanyDTO
    {
        public int CompanyId { get; set; }
        public Guid CompanyGuid { get; set; }
        public String Name { get; set; }
        public String PhoneNumber { get; set; }
        public String FaxNumber { get; set; }
        public String Address { get; set; }
        public String Website { get; set; }
        public String Email { get; set; }
        public String PersonalNumber { get; set; }
        public String PIB { get; set; }
        public String MIB { get; set; }
        public String AccountNumber { get; set; }
        public String BankCode { get; set; }
        public DateTime DeleteDate { get; set; }
    }
}
