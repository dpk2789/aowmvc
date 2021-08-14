using AowCore.Application.IRepository;
using AowCore.Domain;
using AowCore.Infrastructure.Common;
using AowCore.Infrastructure.Data;

namespace AowCore.Infrastructure.Repositories
{
    public class VoucherSundryItemRepository : GenericRepository<VoucherSundryItem>, IVoucherSundryItemRepository
    {
        public VoucherSundryItemRepository(ApplicationDbContext context) : base(context) { }
    }
}
