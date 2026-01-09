using ManhwaDimension.Models;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT;
using System.Threading.Tasks;

namespace ManhwaDimension.Service
{
    public class ComicService : IComicService
    {
        private readonly IComicRepository _comicRepository;
        private readonly ICloudflareR2Client _cloudflareR2Client;

        public ComicService(IComicRepository comicRepository, ICloudflareR2Client cloudflareR2Client)
        {
            _comicRepository = comicRepository;
            _cloudflareR2Client = cloudflareR2Client;
        }

        public async Task<Comic> AddComicIMG(Comic obj, IFormFile file)
        {
            if(file != null)
            {
                // Upload ảnh lên Cloudflare R2 và lấy URL của ảnh
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);    
                var imgUrl = await _cloudflareR2Client.UploadFileAsync(file.OpenReadStream(), fileName);
                // Gán URL của ảnh vào đối tượng Comic
                obj.CoverImageUrl = imgUrl;
            }
            return await _comicRepository.Add(obj);
        }

        public int Count()
        {
            return _comicRepository.Count();
        }

        public async Task Delete(Comic obj)
        {
            await _comicRepository.Delete(obj);
        }

        public Task<int> DeletePermanently(int id)
        {
            return _comicRepository.DeletePermanently(id);
        }

        public Task<Comic> Detail(long id)
        {
            return _comicRepository.Detail(id);
        }

        public async Task<DTResult<Comic>> ListServerSide(ComicDTParameters parameters)
        {
           return await _comicRepository.ListServerSide(parameters);   
        }

        public Task<List<Comic>> List()
        {
            return _comicRepository.List(); 
        }

        public Task<List<Comic>> ListPaging(int pageIndex, int pageSize)
        {
            return _comicRepository.ListPaging(pageIndex, pageSize);
        }

        public async Task<List<Comic>> Search(string keyword)
        {
            return await _comicRepository.Search(keyword);
        }

        public async Task Update(Comic obj)
        {
            await _comicRepository.Update(obj);   
        }

        public Task Add(Comic obj)
        {
            throw new NotImplementedException();
        }
    }
}
