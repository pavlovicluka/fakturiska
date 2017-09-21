using Fakturiska.Business.DTOs;
using Fakturiska.Business.Logic;
using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fakturiska.Models
{
    public class CompanyModel
    {
        public Guid? CompanyGuid { get; set; }

        [DisplayName("Naziv")]
        [RequiredIf("CompanyEmpty", false, ErrorMessage = "Polje Naziv je obavezno")]
        public String Name { get; set; }
        [DisplayName("Telefon")]
        [RequiredIf("CompanyEmpty", false, ErrorMessage = "Polje Telefon je obavezno")]
        [Phone]
        public String PhoneNumber { get; set; }
        [DisplayName("Fax")]
        [RequiredIf("CompanyEmpty", false, ErrorMessage = "Polje Fax je obavezno")]
        [Phone]
        public String FaxNumber { get; set; }
        [DisplayName("Adresa")]
        [RequiredIf("CompanyEmpty", false, ErrorMessage = "Polje Adresa je obavezno")]
        public String Address { get; set; }
        [DisplayName("Website")]
        [RequiredIf("CompanyEmpty", false, ErrorMessage = "Polje Website je obavezno")]
        [Url]
        public String Website { get; set; }
        [DisplayName("Email")]
        [RequiredIf("CompanyEmpty", false, ErrorMessage = "Polje Email je obavezno")]
        [EmailAddress]
        public String Email { get; set; }
        [DisplayName("Maticni broj")]
        [RequiredIf("CompanyEmpty", false, ErrorMessage = "Polje Maticni broj je obavezno")]
        public String PersonalNumber { get; set; }
        [DisplayName("PIB")]
        [RequiredIf("CompanyEmpty", false, ErrorMessage = "Polje PIB je obavezno")]
        public String PIB { get; set; }
        [DisplayName("MIB")]
        [RequiredIf("CompanyEmpty", false, ErrorMessage = "Polje MIB je obavezno")]
        public String MIB { get; set; }
        [DisplayName("Broj tekućeg računa")]
        [RequiredIf("CompanyEmpty", false, ErrorMessage = "Polje Broj tekućeg računa je obavezno")]
        public String AccountNumber { get; set; }
        [DisplayName("Šifra za uplatu")]
        [RequiredIf("CompanyEmpty", false, ErrorMessage = "Polje Šifra za uplatu je obavezno")]
        public String BankCode { get; set; }

        [NotMapped]
        public bool CompanyEmpty
        {
            get
            {
                return ((Name == "" || Name == null)
                    && (PhoneNumber == "" || PhoneNumber == null)
                    && (FaxNumber == "" || FaxNumber == null)
                    && (Address == "" || Address == null)
                    && (Website == "" || Website == null)
                    && (Email == "" || Email == null)
                    && (PersonalNumber == "" || PersonalNumber == null)
                    && (PIB == "" || PIB == null)
                    && (MIB == "" || MIB == null)
                    && (AccountNumber == "" || AccountNumber == null)
                    && (BankCode == "" || BankCode == null));
            }
        }

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

        public CompanyModel(CompanyDTO company)
        {
            CompanyGuid = company.CompanyGuid;
            Name = company.Name;
            PhoneNumber = company.PhoneNumber;
            FaxNumber = company.FaxNumber;
            Address = company.Address;
            Website = company.Website;
            Email = company.Email;
            PersonalNumber = company.PersonalNumber;
            PIB = company.PIB;
            MIB = company.MIB;
            AccountNumber = company.AccountNumber;
            BankCode = company.BankCode;
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

        public static CompanyDTO MapModelToDTO(CompanyModel company)
        {
            CompanyDTO companyDTO = new CompanyDTO();
            if (company.CompanyGuid != null && company.CompanyGuid != Guid.Empty)
                companyDTO.CompanyGuid = (Guid)company.CompanyGuid;
            companyDTO.Name = company.Name;
            companyDTO.PhoneNumber = company.PhoneNumber;
            companyDTO.FaxNumber = company.FaxNumber;
            companyDTO.Address = company.Address;
            companyDTO.Website = company.Website;
            companyDTO.Email = company.Email;
            companyDTO.PersonalNumber = company.PersonalNumber;
            companyDTO.PIB = company.PIB;
            companyDTO.MIB = company.MIB;
            companyDTO.AccountNumber = company.AccountNumber;
            companyDTO.BankCode = company.BankCode;

            return companyDTO;
        }
    }
}