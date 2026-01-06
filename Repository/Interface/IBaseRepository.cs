using NPOI.HSSF.Model;

namespace ManhwaDimension.Repository.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> List();
        Task<List<T>> Search(string keyword);
        Task<List<T>> ListPaging(int pageIndex, int pageSize);
        Task<T> Detail(long id);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<int> DeletePermanently(int id);
        int Count();
    }
}
