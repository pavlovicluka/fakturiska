using Fakturiska.Business.DTOs;
using Fakturiska.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fakturiska.Business.Logic
{
    public class CompanyLogic
    {
        public static int? CreateCompany(CompanyDTO company)
        {
            if(company.Name == null)
            {
                return null;
            }

            int companyId = CompanyExists(company);
            if (companyId == 0)
            {
                Company com = new Company()
                {
                    CompanyUId = Guid.NewGuid(),
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
                };

                using (var dc = new FakturiskaDBEntities())
                {
                    dc.Companies.Add(com);
                    dc.SaveChanges();

                    companyId =  dc.Companies.FirstOrDefault(c => c.CompanyUId == com.CompanyUId).CompanyId;
                }
            }
            return companyId;
        }

        public static int CompanyExists(CompanyDTO company)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var com = dc.Companies.FirstOrDefault(c => c.Name == company.Name);
                if(com != null)
                {
                    return com.CompanyId;
                }
                return 0;
            }
        }

        public static CompanyDTO GetCompanyByGuid(Guid companyGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var company = dc.Companies.FirstOrDefault(c => c.CompanyUId == companyGuid);
                return new CompanyDTO(company);
            }
        }

        public static CompanyDTO GetCompanyById(int companyId)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var company = dc.Companies.FirstOrDefault(c => c.CompanyId == companyId);
                return new CompanyDTO(company);
            }
        }

        public static void EditCompany(CompanyDTO company)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var c = GetCompanyById(company.CompanyGuid, dc);
                if (c != null)
                {
                    c.Name = company.Name;
                    c.PhoneNumber = company.PhoneNumber;
                    c.FaxNumber = company.FaxNumber;
                    c.Address = company.Address;
                    c.Website = company.Website;
                    c.Email = company.Email;
                    c.PersonalNumber = company.PersonalNumber;
                    c.PIB = company.PIB;
                    c.MIB = company.MIB;
                    c.AccountNumber = company.AccountNumber;
                    c.BankCode = company.BankCode;
                }
                dc.SaveChanges();
            }
        }

        public static void DeleteCompany(Guid companyGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var company = GetCompanyById(companyGuid, dc);
                if (company != null)
                {
                    company.DeleteDate = DateTime.Now;
                }
                dc.SaveChanges();
            }
        }

        public static IEnumerable<CompanyDTO> GetAllCompanies()
        {
            List<CompanyDTO> companyDTOs = null;
            using (var dc = new FakturiskaDBEntities())
            {
                var companies = dc.Companies.Where(company => company.DeleteDate == null).ToList();
                companyDTOs = companies.Select(company => new CompanyDTO(company)).ToList();
            }
            return companyDTOs;
        }

        public static IEnumerable<CompanyDTO> GetAllCompaniesAutocomplete(string prefix)
        {
            prefix = prefix.ToLower();
            List<CompanyDTO> companyDTOs = null;
            using (var dc = new FakturiskaDBEntities())
            {
                var companies = dc.Companies.Where(company => company.Name.ToLower().Trim().Contains(prefix) && company.DeleteDate == null).ToList();
                companyDTOs = companies.Select(company => new CompanyDTO(company)).ToList();
            }
            return companyDTOs;
        }

        private static Company GetCompanyById(Guid companyGuid, FakturiskaDBEntities dc)
        {
            return dc.Companies.FirstOrDefault(c => c.CompanyUId == companyGuid && c.DeleteDate == null);
        }
    }
}
