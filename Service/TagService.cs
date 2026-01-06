using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT;
using ManhwaDimension.Util.Extentions;
using Quartz.Util;

namespace ManhwaDimension.Service
{
    public class TagService : BaseService<Tag>, ITagService
    {
        ITagRepository repositoryTag;
        public TagService(IBaseRepository<Tag> _repository, ITagRepository _repositoryTag) : base(_repository)
        {
            repositoryTag = _repositoryTag;
        }

        public async Task<DTResult<Tag>> ListServerSide(TagDTParameters parameters)
        {
            return await repositoryTag.ListServerSide(parameters);
        }
        public override async Task Add(Tag obj)
        {
            obj.Slug = StringExtension.ConvertToSlug(obj.Name);
            await repository.Add(obj);
        }
        public override async Task Update(Tag obj)
        {
            obj.Slug = StringExtension.ConvertToSlug(obj.Name);
            await base.Update(obj);
        }
    }
}
