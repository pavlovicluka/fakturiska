using Fakturiska.Business.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fakturiska.Models
{
    public class CompanyModel
    {
        public Guid CompanyGuid { get; set; }

        [DisplayName("Naziv")]
        [Required()]
        public String Name { get; set; }
        [DisplayName("Telefon")]
        [Required()]
        [Phone]
        public String PhoneNumber { get; set; }
        [DisplayName("Fax")]
        [Required()]
        [Phone]
        public String FaxNumber { get; set; }
        [DisplayName("Adresa")]
        [Required()]
        public String Address { get; set; }
        [DisplayName("Website")]
        [Required()]
        [Url]
        public String Website { get; set; }
        [DisplayName("Email")]
        [Required()]
        [EmailAddress]
        public String Email { get; set; }
        [DisplayName("Maticni broj")]
        [Required()]
        public String PersonalNumber { get; set; }
        [DisplayName("PIB")]
        [Required()]
        public String PIB { get; set; }
        [DisplayName("MIB")]
        [Required()]
        public String MIB { get; set; }
        [DisplayName("Broj tekućeg računa")]
        [Required()]
        public String AccountNumber { get; set; }
        [DisplayName("Šifra za uplatu")]
        [Required()]
        public String BankCode { get; set; }

        public CompanyModel()
        {

        }

        public CompanyModel(Guid id)
        {
            var company = CompanyLogic.GetCompanyByGuid(id);
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