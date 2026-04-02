using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.ULT;

namespace ManhwaDimension.Service.Interface
{
    public interface IChapterService : IBaseService<Chapter>
    {
        Task<DTResult<Chapter>> ListServerSide(ChapterDTParameters parameters);
    }
}
