using ManhwaDimension.Models;
using ManhwaDimension.ULT;

namespace ManhwaDimension.Repository.Interface
{
    public interface IChapterRepository : IBaseRepository<Chapter>
    {
        Task<DTResult<Chapter>> ListServerSide(ChapterDTParameters parameters);
    }
}
