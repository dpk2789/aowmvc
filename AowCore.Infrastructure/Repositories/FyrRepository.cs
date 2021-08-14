using AowCore.Application.IRepository;
using AowCore.Domain;
using AowCore.Infrastructure.Common;
using AowCore.Infrastructure.Data;

namespace AowCore.Infrastructure.Repositories
{
    public class FyrRepository : GenericRepository<FinancialYear>, IFyrRepository
    {
        public FyrRepository(ApplicationDbContext context) : base(context) { }
    }
}
