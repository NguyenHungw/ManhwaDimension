using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.ULT;


namespace ManhwaDimension.Service.Interface
{
    public interface ISubscriptionService : IBaseService<Subscription>
    {
        Task<DTResult<Subscription>> ListServerSide(SubscriptionDTParameters parameters);
    }
}
