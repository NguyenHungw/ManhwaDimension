using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.ULT;


namespace ManhwaDimension.Service.Interface
{
    public interface ITagService : IBaseService<Tag>
    {
        Task<DTResult<Tag>> ListServerSide(TagDTParameters parameters);
    }
}
