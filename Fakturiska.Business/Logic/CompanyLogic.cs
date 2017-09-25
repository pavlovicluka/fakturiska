using Fakturiska.Business.DTOs;
using Fakturiska.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Fakturiska.Business.Logic
{
    public class CompanyLogic
    {
        public static Dictionary<string, int?> CreateCompany(CompanyDTO company, string type = "")
        {
            Dictionary<string, int?> response = null;
            using (var dc = new FakturiskaDBEntities())
            {
                response = CompanyExists(company, type, dc);
                if (response.Count == 0)
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

                    dc.Companies.Add(com);
                    dc.SaveChanges();

                    response.Add("company" + type, dc.Companies.FirstOrDefault(c => c.CompanyUId == com.CompanyUId).CompanyId);
                }
            }
            return response;
        }

        public static Dictionary<string, int?> CreateCompany(CompanyDTO company, FakturiskaDBEntities dc, string type = "")
        {
            Dictionary<string, int?> response = CompanyExists(company, type, dc);
            if (response.Count == 0)
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
                dc.Companies.Add(com);
                response.Add("companyId", dc.Companies.FirstOrDefault(c => c.CompanyUId == com.CompanyUId).CompanyId);
            }
            return response;
        }

        public static Dictionary<string, int?> EditCompany(CompanyDTO company, string type = "")
        {
            Dictionary<string, int?> response = null;
            using (var dc = new FakturiskaDBEntities())
            {
                response = CompanyExists(company, type, dc);
                if (response.Count == 0)
                {
                    var c = GetCompanyByGuid(company.CompanyGuid, dc);
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

                    response.Add("company" + type, c.CompanyId);
                }
            }
            return response;
        }

        public static void DeleteCompany(Guid companyGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var company = GetCompanyByGuid(companyGuid, dc);
                if (company != null)
                {
                    company.DeleteDate = DateTime.Now;
                }
                dc.SaveChanges();
            }
        }

        public static Dictionary<string, int?> CheckCompany(CompanyDTO company, FakturiskaDBEntities dc, string type = "")
        {
            Dictionary<string, int?> response = new Dictionary<string, int?>();

            var c = GetCompanyByGuid(company.CompanyGuid, dc);

            if (c != null)
            {
                if (company.Name == "" || company.Name == null)
                {
                    response.Add("company" + type, null);
                }
                if (company.Name != c.Name || company.PersonalNumber != c.PersonalNumber || company.PIB != c.PIB)
                {
                    response.Add("company" + type + "cannotEdit", 1); ;
                }
                else
                {
                    response.Add("company" + type, c.CompanyId);
                }
            } else {
                response.Add("company" + type, 0);
            }

            return response;
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

        public static List<CompanyDTO> GetCompanies(string searchBy, int take, int skip, string sortBy, string sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            if(searchBy != null)
            {
                searchBy.Trim().ToLower();
            } else
            {
                searchBy = "";
            }

            if (String.IsNullOrEmpty(sortBy))
            {
                sortBy = "CompanyId";
                sortDir = "asc";
            }

            List<CompanyDTO> companyDTOs = null;
            using (var dc = new FakturiskaDBEntities())
            {
                var companies = dc.Companies
                            .Where(c => c.DeleteDate == null && (c.Name.ToLower().Contains(searchBy)
                                                             || c.PhoneNumber.ToLower().Contains(searchBy)
                                                             || c.FaxNumber.ToLower().Contains(searchBy)
                                                             || c.Address.ToLower().Contains(searchBy)
                                                             || c.Website.ToLower().Contains(searchBy)
                                                             || c.Email.ToLower().Contains(searchBy)
                                                             || c.PersonalNumber.ToLower().Contains(searchBy)
                                                             || c.PIB.ToLower().Contains(searchBy)
                                                             || c.MIB.ToLower().Contains(searchBy)
                                                             || c.AccountNumber.ToLower().Contains(searchBy)
                                                             || c.BankCode.ToLower().Contains(searchBy)))
                           .OrderBy(sortBy + " " + sortDir)
                           .Skip(skip)
                           .Take(take)
                           .ToList();
                companyDTOs = companies.Select(company => new CompanyDTO(company)).ToList();

                filteredResultsCount = dc.Companies
                                        .Where(c => c.DeleteDate == null && (c.Name.ToLower().Contains(searchBy)
                                                             || c.PhoneNumber.ToLower().Contains(searchBy)
                                                             || c.FaxNumber.ToLower().Contains(searchBy)
                                                             || c.Address.ToLower().Contains(searchBy)
                                                             || c.Website.ToLower().Contains(searchBy)
                                                             || c.Email.ToLower().Contains(searchBy)
                                                             || c.PersonalNumber.ToLower().Contains(searchBy)
                                                             || c.PIB.ToLower().Contains(searchBy)
                                                             || c.MIB.ToLower().Contains(searchBy)
                                                             || c.AccountNumber.ToLower().Contains(searchBy)
                                                             || c.BankCode.ToLower().Contains(searchBy)))
                                        .Count();
                totalResultsCount = dc.Companies.Where(company => company.DeleteDate == null).Count();
            }
            return companyDTOs;
        }


        public static IEnumerable<CompanyDTO> GetAllCompaniesAutocomplete(string prefix, int fieldCase)
        {
            prefix = prefix.Trim().ToLower();
            List<CompanyDTO> companyDTOs = null;
            List<Company> companies = null;
            using (var dc = new FakturiskaDBEntities())
            {
                switch(fieldCase)
                {
                    case 1:
                        companies = dc.Companies.Where(company => company.Name.ToLower().Contains(prefix) && company.DeleteDate == null).ToList();
                        break;
                    case 2:
                        companies = dc.Companies.Where(company => company.PersonalNumber.ToLower().Contains(prefix) && company.DeleteDate == null).ToList();
                        break;
                    case 3:
                        companies = dc.Companies.Where(company => company.PIB.ToLower().Contains(prefix) && company.DeleteDate == null).ToList();
                        break;
                }
                companyDTOs = companies.Select(company => new CompanyDTO(company)).ToList();
            }
            return companyDTOs;
        }

        private static Company GetCompanyByGuid(Guid companyGuid, FakturiskaDBEntities dc)
        {
            return dc.Companies.FirstOrDefault(c => c.CompanyUId == companyGuid && c.DeleteDate == null);
        }

        private static Dictionary<string, int?> CompanyExists(CompanyDTO company, string type, FakturiskaDBEntities dc)
        {
            Dictionary<string, int?> response = new Dictionary<string, int?>();

            if (company.Name == "" || company.Name == null)
            {
                response.Add("company" + type, null);
            }
            else
            {
                var com = dc.Companies.FirstOrDefault(c => c.Name.Trim().ToLower() == company.Name.Trim().ToLower() && c.CompanyUId != company.CompanyGuid);
                if (com != null)
                {
                    response.Add("company" + type + "NameExists", 1);
                }
                com = dc.Companies.FirstOrDefault(c => c.PersonalNumber.Trim().ToLower() == company.PersonalNumber.Trim().ToLower() && c.CompanyUId != company.CompanyGuid);
                if (com != null)
                {
                    response.Add("company" + type + "PersonalNumberExists", 1);
                }
                com = dc.Companies.FirstOrDefault(c => c.PIB.Trim().ToLower() == company.PIB.Trim().ToLower() && c.CompanyUId != company.CompanyGuid);
                if (com != null)
                {
                    response.Add("company" + type + "PIBExists", 1);
                }
            }
            return response;
        }
    }
}
