using AowCore.Application.IRepository;
using AowCore.Domain;
using AowCore.Infrastructure.Common;
using AowCore.Infrastructure.Data;

namespace AowCore.Infrastructure.Repositories
{
    public class JournalEntryRepository : GenericRepository<JournalEntry>, IJournalEntryRepository
    {
        public JournalEntryRepository(ApplicationDbContext context) : base(context) { }
    }
}
