using ManhwaDimension.Models;
using ManhwaDimension.ULT;

namespace ManhwaDimension.Repository.Interface
{
    public interface IGenreRepository : IBaseRepository<Genre>
    {
        Task<DTResult<Genre>> ListServerSide(GenreDTParameters parameters);

    }
}
