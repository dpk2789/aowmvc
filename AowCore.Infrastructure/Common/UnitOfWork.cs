using AowCore.Application;
using AowCore.Infrastructure.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AowCore.Infrastructure.Common
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private ApplicationDbContext dbContext;

        private bool disposed = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            int result = await dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposed)
        {
            if (!disposed)
            {
                if (!isDisposed)
                {
                    dbContext.Dispose();
                }
            }

            disposed = true;
        }
    }
}
