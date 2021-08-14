using AowCore.Application.IRepository;
using AowCore.Domain.Region;
using AowCore.Infrastructure.Common;
using AowCore.Infrastructure.Data;

namespace AowCore.Infrastructure.Repositories
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        public CountryRepository(ApplicationDbContext context) : base(context) { }
    }
}
