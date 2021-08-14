using AowCore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace AowCore.Application
{
    public interface ItestRepo
    {
        void AddCompany(Company company);
        Task<IReadOnlyList<Company>> GetAllCompanies();
        Task<Company> GetCompany(Guid id);
        void DeleteCompany(Company company);
        void UpdateCompany(Company company);
    }
}
