using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT;

namespace ManhwaDimension.Service
{
    public class ChapterService : BaseService<Chapter>, IChapterService
    {
        IChapterRepository repositoryChapter;
        public ChapterService(IBaseRepository<Chapter> _repository, IChapterRepository _repositoryChapter) : base(_repository)
        {
            repositoryChapter = _repositoryChapter;
        }

        public async Task<DTResult<Chapter>> ListServerSide(ChapterDTParameters parameters)
        {
            return await repositoryChapter.ListServerSide(parameters);
        }
    }
}
