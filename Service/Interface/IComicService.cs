using ManhwaDimension.Models;
using ManhwaDimension.Models.DTO.Comic;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.ULT;


namespace ManhwaDimension.Service.Interface
{
    public interface IComicService : IBaseService<Comic>
    {
        Task<DTResult<Comic>> ListServerSide(ComicDTParameters parameters);
        Task<Comic> AddComicIMG (ComicDTO comic, IFormFile imgFile);
    }
}
