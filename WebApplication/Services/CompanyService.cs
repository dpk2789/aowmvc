using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AowCore.Application;
using AowCore.Domain;
using AowCore.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class CompanyService : BaseManager, ICompanyService
    {

        private readonly IApplicationDbContext _context;
        public CompanyService(IApplicationDbContext context, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            //_posts = new List<Posts>();
            //for (int i = 0; i < 5; i++)
            //{
            //    _posts.Add(new Posts { Id = Guid.NewGuid(), Name = $"Post Name{i}" });
            //}           
            _context = context;
        }

        public async Task<IReadOnlyList<Company>> GetAll()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company> GetPostById(Guid postId)
        {
            return await _context.Companies.FirstOrDefaultAsync(e => e.Id == postId);
        }

        public async Task<bool> CreatePost(Company company, CancellationToken cancellationToken)
        {
            await _context.Companies.AddAsync(company);
            //  var updated = await _context.SaveChangesAsync(cancellationToken);
            var updated = await this.UnitOfWork.Commit(cancellationToken);
            return updated > 0;
        }


        public async Task<bool> UpdatePost(Company updatePost, CancellationToken cancellationToken)
        {
            _context.Companies.Update(updatePost);
            var updated = await _context.SaveChangesAsync(cancellationToken);
            return updated > 0;
        }

        public async Task<bool> DeletePost(Company cmp, CancellationToken cancellationToken)
        {
            _context.Companies.Remove(cmp);
            var deleted = await _context.SaveChangesAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
