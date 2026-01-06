using iText.StyledXmlParser.Jsoup.Parser;
using ManhwaDimension.Models;
using ManhwaDimension.ULT;

namespace ManhwaDimension.Repository.Interface
{
    public interface ITagRepository : IBaseRepository<Models.Tag>
    {
        Task<DTResult<Models.Tag>> ListServerSide(TagDTParameters parameters);

    }
}
