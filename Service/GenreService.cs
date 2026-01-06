using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT;
using ManhwaDimension.Util.Entities;
using NPOI.SS.Formula.Functions;
using ManhwaDimension.Util.Extentions;
namespace ManhwaDimension.Service
{
    public class GenreService : BaseService<Genre>, IGenreService
    {
        IGenreRepository repositoryGenre;
        public GenreService(IBaseRepository<Genre> _repository, IGenreRepository _repositoryGenre) : base(_repository)
        {
            repositoryGenre = _repositoryGenre;
        }

        public async Task<DTResult<Genre>> ListServerSide(GenreDTParameters parameters)
        {
            return await repositoryGenre.ListServerSide(parameters);
        }
        public override async Task Add(Genre genre)
        {
            genre.Slug = StringExtension.ConvertToSlug(genre.Name);
    
            await repositoryGenre.Add(genre);

        }
        public override async Task Update(Genre genre)
        {
            genre.Slug = StringExtension.ConvertToSlug(genre.Name);
            await repositoryGenre.Update(genre);
        }
    }
}
