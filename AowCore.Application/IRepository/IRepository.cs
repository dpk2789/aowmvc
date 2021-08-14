using AowCore.Domain.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AowCore.Application
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAllEnumerableList();
        IQueryable<T> GetAll();
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> FindAsync(object id);
        T Find(object id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsyncNew();
    }
}
