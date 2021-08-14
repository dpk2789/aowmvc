using AowCore.Application.IRepository;
using AowCore.Domain;
using AowCore.Infrastructure.Common;
using AowCore.Infrastructure.Data;

namespace AowCore.Infrastructure.Repositories
{
    public class VoucherItemRepository : GenericRepository<VoucherItem>, IVoucherItemRepository
    {
        public VoucherItemRepository(ApplicationDbContext context) : base(context) { }
    }
}
