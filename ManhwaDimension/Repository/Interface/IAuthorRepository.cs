using ManhwaDimension.Models;
using ManhwaDimension.ULT;

namespace ManhwaDimension.Repository.Interface
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        Task<DTResult<Author>> ListServerSide(AuthorDTParameters parameters);

    }
}
