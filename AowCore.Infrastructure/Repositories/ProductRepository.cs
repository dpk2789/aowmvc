using AowCore.Application.IRepository;
using AowCore.Domain;
using AowCore.Infrastructure.Common;
using AowCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AowCore.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetProducts(Guid cmpidG)
        {
            var products = await context.Products.Include(p => p.ProductCategory).Where(x => x.ProductCategory.CompanyId == cmpidG).OrderBy(x => x.Name).ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByTerm(Guid cmpidG, string term)
        {
            var products = await context.Products.Include(x => x.ProductCategory).Where(c => c.ProductCategory.CompanyId == cmpidG).
                Where(ii => ii.Name.Contains(term)).OrderBy(x => x.Name).ToListAsync();
            return products;
        }

        public async Task<Product> GetProductById(Guid id)
        {
            var product = await context.Products.Include(i => i.ProductCategory).FirstOrDefaultAsync(i => i.Id == id);
            return product;
        }

        public bool ProductExistsAny(Guid id)
        {
            return context.Products.Any(e => e.Id == id);
        }

    }
}
