using AowCore.Application;
using AowCore.Application.IRepository;
using AowCore.Application.IServices;
using AowCore.Infrastructure.Common;

namespace AowCore.Infrastructure.Services
{
    public class ProductService : BaseManager, IProductService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public ProductService(IApplicationDbContext context, IUnitOfWork unitOfWork, IProductRepository productRepository) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;

        }

      
    }
}
