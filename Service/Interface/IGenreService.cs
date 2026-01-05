using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.ULT;


namespace ManhwaDimension.Service.Interface
{
    public interface IGenreService : IBaseService<Genre>
    {
        Task<DTResult<Genre>> ListServerSide(GenreDTParameters parameters);

    }
}
