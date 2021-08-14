

using AowCore.Application;

namespace AowCore.Infrastructure.Common
{
    public class BaseManager
    {
        private readonly IUnitOfWork unitOfWork;

        public BaseManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        protected IUnitOfWork UnitOfWork => unitOfWork;
    }
}
