using AowCore.Application.IRepository;
using AowCore.Domain;
using AowCore.Infrastructure.Common;
using AowCore.Infrastructure.Data;

namespace AowCore.Infrastructure.Repositories
{
    public class LedgerCategoryRepository : GenericRepository<LedgerCategory>, ILedgerCategoryRepository
    {
        public LedgerCategoryRepository(ApplicationDbContext context) : base(context) { }
    }
}
