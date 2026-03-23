using Microsoft.EntityFrameworkCore;

namespace ManhwaDimension.Repository.Interface
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        Task<int> CommitAsync();
    }
}
