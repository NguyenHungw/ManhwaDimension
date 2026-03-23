using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT;
using ManhwaDimension.Util.Extentions;
using Quartz.Util;

namespace ManhwaDimension.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        ISubscriptionRepository repositorySubscription;
        public SubscriptionService(ISubscriptionRepository _repositorySubscription)
        {
            repositorySubscription = _repositorySubscription;
        }

        public async Task Add(Subscription obj)
        {
            await repositorySubscription.Add(obj);
        }

        public int Count()
        {
            return repositorySubscription.Count();
        }

        public async Task Delete(Subscription obj)
        {
            await repositorySubscription.Delete(obj);
        }

        public async Task<int> DeletePermanently(int id)
        {
            return await repositorySubscription.DeletePermanently(id);
        }

        public async Task<Subscription> Detail(long id)
        {
            return await repositorySubscription.Detail(id);
        }

        public async Task<List<Subscription>> List()
        {
            return await repositorySubscription.List();
        }

        public async Task<List<Subscription>> ListPaging(int pageIndex, int pageSize)
        {
            return await repositorySubscription.ListPaging(pageIndex, pageSize);
        }

        public async Task<DTResult<Subscription>> ListServerSide(SubscriptionDTParameters parameters)
        {
            return await repositorySubscription.ListServerSide(parameters);
        }

        public async Task<List<Subscription>> Search(string keyword)
        {
            return await repositorySubscription.Search(keyword);
        }

        public async Task Update(Subscription obj)
        {
            await repositorySubscription.Update(obj);
        }
    }
}
