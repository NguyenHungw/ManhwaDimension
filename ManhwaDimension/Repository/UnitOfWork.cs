using ManhwaDimension.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ManhwaDimension.Repository
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext context;
        public UnitOfWork(TContext _context)
        {
            context = _context;

        }
        public async Task<int> CommitAsync() => await context.SaveChangesAsync();

        public void Dispose() => context.Dispose();

    }
}
