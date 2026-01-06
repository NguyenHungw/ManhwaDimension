using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT;
using ManhwaDimension.Util.Entities;
using NPOI.SS.Formula.Functions;
using ManhwaDimension.Util.Extentions;
namespace ManhwaDimension.Service
{
    public class AuthorService : BaseService<Author>, IAuthorService
    {
        IAuthorService repositorAuthor;
        public AuthorService(IBaseRepository<Author> _repository, IAuthorService _repositorAuthor) : base(_repository)
        {
            repositorAuthor = _repositorAuthor;
        }

        public async Task<DTResult<Author>> ListServerSide(AuthorDTParameters parameters)
        {
            return await repositorAuthor.ListServerSide(parameters);
        }
        public override async Task Add(Author item)
        {
            item.Slug = StringExtension.ConvertToSlug(item.Name);
    
            await repositorAuthor.Add(item);

        }
        public override async Task Update(Author item)
        {
            item.Slug = StringExtension.ConvertToSlug(item.Name);
            await repositorAuthor.Update(item);
        }
    }
}
