using System.Threading;
using System.Threading.Tasks;

namespace AowCore.Application
{
    public interface IUnitOfWork
    {
        Task<int> Commit(CancellationToken cancellationToken);
    }
}
