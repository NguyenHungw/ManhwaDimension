using ManhwaDimension.Models;
using ManhwaDimension.ULT;

namespace ManhwaDimension.Repository.Interface
{
    public interface IComicRepository : IBaseRepository<Comic>
    {
        Task<DTResult<Comic>> ListServerSide(ComicDTParameters parameters);

    }
}
