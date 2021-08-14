using AowCore.Domain;
using AowCore.Application;
using AowCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace AowCore.Infrastructure.Repositories
{
    public class testRepo : ItestRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Company> _companyEntity;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        public testRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
        {
            this._context = context;
            _companyEntity = context.Set<Company>();
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<IReadOnlyList<Company>> GetAllCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company> GetCompany(Guid id)
        {
            return await _companyEntity.SingleOrDefaultAsync(s => s.Id == id);
        }
        public void AddCompany(Company company)
        {
            if (company == null) throw new ArgumentNullException("entity");          
            _context.Entry(company).State = EntityState.Added;
        }
        public void UpdateCompany(Company company)
        {
            if (company == null) throw new ArgumentNullException("entity");
            _context.Entry(company).State = EntityState.Modified;
        }

        public void DeleteCompany(Company company)
        {
            _companyEntity.Remove(company);
        }

    }
}
