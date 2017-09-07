using Fakturiska.Business.DTOs;
using Fakturiska.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    CompanyUId = company.CompanyGuid,
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

                    try
                    {
                        dc.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                    companyId =  dc.Companies.Where(c => c.CompanyUId == com.CompanyUId).FirstOrDefault().CompanyId;
                }
            }
            return companyId;
        }

        public static int CompanyExists(CompanyDTO company)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var com = dc.Companies.Where(c => c.Name == company.Name).FirstOrDefault();
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
                var company = dc.Companies.Where(c => c.CompanyUId == companyGuid).FirstOrDefault();
                return new CompanyDTO
                {
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
            }
        }

        public static CompanyDTO GetCompanyById(int companyId)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var company = dc.Companies.Where(c => c.CompanyId == companyId).FirstOrDefault();
                return new CompanyDTO
                {
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
            List<CompanyDTO> companyDTO = null;
            using (var dc = new FakturiskaDBEntities())
            {
                companyDTO = dc.Companies.Where(company => company.DeleteDate == null).Select(company => new CompanyDTO
                {
                    CompanyGuid = company.CompanyUId,
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
                }).ToList();
            }
            return companyDTO;
        }

        public static IEnumerable<CompanyDTO> GetAllCompaniesAutocomplete(string prefix)
        {
            prefix = prefix.ToLower();
            List<CompanyDTO> companyDTOs = new List<CompanyDTO>();
            using (var dc = new FakturiskaDBEntities())
            {
                List<Company> companies = dc.Companies.Where(company => company.Name.ToLower().Trim().StartsWith(prefix) && company.DeleteDate == null).ToList();
                foreach (var company in companies)
                {
                    companyDTOs.Add(new CompanyDTO
                    {
                        CompanyId = company.CompanyId,
                        CompanyGuid = company.CompanyUId,
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
            return companyDTOs;
        }

        private static Company GetCompanyById(Guid companyGuid, FakturiskaDBEntities dc)
        {
            return dc.Companies.FirstOrDefault(c => c.CompanyUId == companyGuid && c.DeleteDate == null);
        }
    }
}
