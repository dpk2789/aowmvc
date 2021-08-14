using AowCore.Application.IRepository;
using AowCore.Domain;
using AowCore.Infrastructure.Common;
using AowCore.Infrastructure.Data;

namespace AowCore.Infrastructure.Repositories
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(ApplicationDbContext context) : base(context) { }
    }
}
