using AowCore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace AowCore.Application.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company> GetByName(string firstName);
        Task<Company> GetByIdAsync(Guid Id);      
    }
}
