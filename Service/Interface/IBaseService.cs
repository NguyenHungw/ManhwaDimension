namespace ManhwaDimension.Repository.Interface
{
    public interface IBaseService<TModel> where TModel : class
    {
        Task<List<TModel>> List();
        Task<List<TModel>> Search(string keyword);
        Task<List<TModel>> ListPaging(int pageIndex, int pageSize);
        Task<List<TModel>> Detail(long id);
        Task Add(TModel model);
        Task Delete(long id);

        Task Update(TModel model);

        Task<long> DeletePermanetly(long id);
        int Count();


    }
}
