using AowCore.Application;
using AowCore.Domain.Common;
using AowCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AowCore.Infrastructure.Common
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IEnumerable<T> GetAllEnumerableList()
        {
            return entities.AsEnumerable();
        }
        public IQueryable<T> GetAll()
        {
            return context.Set<T>().AsNoTracking();
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await context.Set<T>().AsNoTracking().ToListAsync();
        }
        public virtual async Task<T> FindAsync(object id)
        {
            return await context.Set<T>().FindAsync(id);
        }       
        public virtual T Find(object id)
        {
            return entities.Find(id);
        }
        public async Task AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            await context.Set<T>().AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Remove(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            context.Set<T>().Remove(entity);
        }

        public async Task<int> SaveChangesAsyncNew()
        {
            return await context.SaveChangesAsync();
        }
    }
}
