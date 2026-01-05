using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Util.Entities;

namespace ManhwaDimension.Service
{
    public class BaseService<T> : IBaseService<T> where T : class, IEntityBase
    {
        protected readonly IBaseRepository<T> repository;
        public BaseService(IBaseRepository<T> _repository)
        {
            repository = _repository;
        }
        public async Task Add(T obj)
        {
            obj.Active = true;
            obj.CreatedTime = DateTime.Now;
            await repository.Add(obj);
        }

        public int Count()
        {
            return repository.Count();
        }

        public async Task Delete(T obj)
        {
            obj.Active = false;
            await repository.Delete(obj);
        }

        public async Task<long> DeletePermanently(long id)
        {
            return await repository.DeletePermanently(id);
        }

        public async Task<T> Detail(long id)
        {
            return await repository.Detail(id);
        }

        public async Task<List<T>> List()
        {
            return await repository.List();
        }

        public async Task<List<T>> ListPaging(int pageIndex, int pageSize)
        {
            return await repository.ListPaging(pageIndex, pageSize);
        }

        public async Task<List<T>> Search(string keyword)
        {
            return await repository.Search(keyword);
        }

        public async Task Update(T obj)
        {
            await repository.Update(obj);
        }
    }
}
