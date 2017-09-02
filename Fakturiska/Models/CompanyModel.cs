using Fakturiska.Business.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakturiska.Models
{
    public class CompanyModel
    {
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

        public CompanyModel()
        {

        }

        public CompanyModel(Guid id)
        {
            var company = CompanyLogic.GetCompanyById(id);
            this.CompanyGuid = id;
            this.Name = company.Name;
            this.PhoneNumber = company.PhoneNumber;
            this.FaxNumber = company.FaxNumber;
            this.Address = company.Address;
            this.Website = company.Website;
            this.Email = company.Email;
            this.PersonalNumber = company.PersonalNumber;
            this.PIB = company.PIB;
            this.MIB = company.MIB;
            this.AccountNumber = company.AccountNumber;
            this.BankCode = company.BankCode;
        }

        public static IEnumerable<CompanyModel> GetAllCompanies()
        {
            return CompanyLogic.GetAllCompanies().Select(company => new CompanyModel
            {
                CompanyGuid = company.CompanyGuid,
                Name = company.Name,
                PhoneNumber = company.PhoneNumber,
                FaxNumber = company.FaxNumber,
                Address = company.Address,
                Website = company.Website,
                Email = company.Email,
                PersonalNumber = company.PersonalNumber,
                PIB = company.PIB,
                MIB = company.MIB,
                AccountNumber = company.AccountNumber,
                BankCode = company.BankCode
            });
        }
    }
}