using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.ULT;


namespace ManhwaDimension.Service.Interface
{
    public interface IAuthorService : IBaseService<Author>
    {
        Task<DTResult<Author>> ListServerSide(AuthorDTParameters parameters);

    }
}
