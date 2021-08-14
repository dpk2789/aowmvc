using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AowCore.Domain;

namespace API.Services
{
    public interface ICompanyService
    {
        Task<IReadOnlyList<Company>> GetAll();
        Task<Company> GetPostById(Guid postId);
        Task<bool> CreatePost(Company updatePost, CancellationToken cancellationToken);
        Task<bool> UpdatePost(Company updatePost, CancellationToken cancellationToken);

        Task<bool> DeletePost(Company deleteCmpany, CancellationToken cancellationToken);

    }
}
