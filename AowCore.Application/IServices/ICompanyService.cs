using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AowCore.Domain;
using AowCore.Domain.Region;

namespace AowCore.Application.Services
{
    public interface ICompanyService
    {
        Task<IReadOnlyList<Company>> GetAll();
        Task<IReadOnlyList<Country>> GetAllCountries();
        Task<Company> GetCompanyById(Guid postId);
        Task<bool> CreateCompany(Company updatePost, string userName, CancellationToken cancellationToken);
        Task<bool> UpdateCompany(Company updatePost, CancellationToken cancellationToken);
        Task<bool> DeleteCompany(Company deleteCmpany, CancellationToken cancellationToken);
        Task<IReadOnlyList<AppUserCompany>> GetCompaniesByUser(string userId);
    }
}
