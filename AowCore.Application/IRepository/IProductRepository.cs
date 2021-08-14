using AowCore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AowCore.Application.IRepository
{
   public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProducts(Guid cmpidG);
        Task<IEnumerable<Product>> GetProductsByTerm(Guid cmpidG, string term);
        Task<Product> GetProductById(Guid id);
        bool ProductExistsAny(Guid id);
    }
}
