using ManhwaDimension.Models;
using ManhwaDimension.ULT;
namespace ManhwaDimension.Repository.Interface
{
    public interface ISubscriptionRepository : IBaseRepository<Subscription>
    {
        Task<DTResult<Subscription>> ListServerSide(SubscriptionDTParameters parameters);
    }
}
