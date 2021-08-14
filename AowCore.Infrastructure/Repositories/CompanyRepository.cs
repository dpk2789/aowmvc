
using AowCore.Application.IRepository;
using AowCore.Domain;
using AowCore.Infrastructure.Common;
using AowCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


namespace AowCore.Infrastructure.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context) { }

        public Task<Company> GetByName(string name)
        {
            return context.Set<Company>().FirstOrDefaultAsync(author => author.CompanyName == name);
        }

        public virtual async Task<Company> GetByIdAsync(Guid Id)
        {
            return await context.Set<Company>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == Id);
            //  return entities.SingleOrDefault(s => s.Id == id);
        }
       

    }
}
