using AutoMapper;
using Humanizer.Localisation;
using ManhwaDimension.Models;
using ManhwaDimension.Models.DTO.Comic;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT;
using ManhwaDimension.Util.Extentions;
using Microsoft.IdentityModel.Logging;
using NPOI.HPSF;
using System.Threading.Tasks;

namespace ManhwaDimension.Service
{
    public class ComicService : IComicService
    {
        private readonly IComicRepository _comicRepository;
        private readonly ICloudflareR2Client _cloudflareR2Client;
        private readonly IMapper _mapper; // Inject AutoMapper

        public ComicService(IComicRepository comicRepository, ICloudflareR2Client cloudflareR2Client, IMapper mapper)
        {
            _comicRepository = comicRepository;
            _cloudflareR2Client = cloudflareR2Client;
            _mapper = mapper;
        }

        public async Task<Comic> AddComicIMG(ComicDTO comicDTO, IFormFile file)
        {
            try
            {
                if (comicDTO == null)
                    throw new ArgumentNullException(nameof(comicDTO));

                // Map từ DTO sang Entity
                var comic = _mapper.Map<Comic>(comicDTO);

                // Set Slug
                var baseSlug = StringExtension.ConvertToSlug(comicDTO.Title);
                comic.Slug = $"{baseSlug}-{DateTime.Now.Ticks}";

                // Upload ảnh nếu có
                if (file != null)
                {
                    var allowExtension = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if(!allowExtension.Contains(extension))
                    {
                        throw new Exception("Invalid file extension. Allowed extensions are: " + string.Join(", ", allowExtension));
                    }
                    if(file.Length > 5 * 1024 * 1024)
                    {
                        throw new Exception("File size exceeds the 5MB limit.");
                    }

                    var fileName = $"Comics/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                    using (var stream = file.OpenReadStream())
                    {
                        comic.CoverImageUrl = await _cloudflareR2Client.UploadFileAsync(stream, fileName);
                    }
                }

                // Lưu vào DB
                await _comicRepository.Add(comic);

                return comic;
            }
            catch (Exception ex)
            {
                // Xóa file đã upload nếu lưu DB thất bại

                //if (!string.IsNullOrEmpty(comic.CoverImageUrl))
                //{
                //    await _cloudflareR2Client.DeleteFileAsync(fileName);
                //}
                //throw;
                throw  new Exception("Error adding comic with image: " + ex.Message, ex);
            }
        }
        public async Task<Comic> UpdateComicIMG(long id, ComicDTO comicDTO, IFormFile file)
        {
            try
            {
                // Lấy comic hiện tại từ DB
                var existingComic = await _comicRepository.Detail(id);
                if (existingComic == null)
                    throw new Exception("Comic not found");

                // Lưu URL ảnh cũ để xóa sau
                var oldImageUrl = existingComic.CoverImageUrl;

                // Map dữ liệu mới (giữ nguyên các trường không update)
                _mapper.Map(comicDTO, existingComic);

                // Upload ảnh mới nếu có
                if (file != null)
                {
                    var allowExtension = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                    if (!allowExtension.Contains(extension))
                        throw new Exception("Invalid file extension. Allowed extensions are: " + string.Join(", ", allowExtension));

                    if (file.Length > 5 * 1024 * 1024)
                        throw new Exception("File size exceeds the 5MB limit.");

                    var fileName = $"Comics/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                    using (var stream = file.OpenReadStream())
                    {
                        existingComic.CoverImageUrl = await _cloudflareR2Client.UploadFileAsync(stream, fileName);
                    }

                    // Xóa ảnh cũ từ R2 nếu upload thành công
                    if (!string.IsNullOrEmpty(oldImageUrl))
                    {
                        try
                        {
                            await _cloudflareR2Client.DeleteFileAsync(ExtractFileNameFromUrl(oldImageUrl));
                        }
                        catch (Exception ex)
                        {
                            // Log lỗi nhưng không throw để không ảnh hưởng đến update
                            Console.WriteLine($"Failed to delete old image: {ex.Message}");
                        }
                    }
                }

                // Cập nhật DB
                await _comicRepository.Update(existingComic);

                return existingComic;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating comic with image: " + ex.Message, ex);
            }
        }

        public async Task DeleteComic(long id)
        {
            try
            {
                var comic = await _comicRepository.Detail(id);
                if (comic == null)
                    throw new Exception("Comic not found");

                // Xóa ảnh từ R2 trước
                if (!string.IsNullOrEmpty(comic.CoverImageUrl))
                {
                    try
                    {
                        await _cloudflareR2Client.DeleteFileAsync(ExtractFileNameFromUrl(comic.CoverImageUrl));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to delete image from R2: {ex.Message}");
                        // Tiếp tục xóa trong DB ngay cả khi xóa R2 thất bại
                    }
                }

                // Xóa khỏi DB
                await _comicRepository.Delete(comic);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting comic: " + ex.Message, ex);
            }
        }

        // Helper method để extract filename từ URL
        private string ExtractFileNameFromUrl(string url)
        {
            // Ví dụ URL: https://your-bucket.r2.cloudflarestorage.com/Comics/abc-123.jpg
            // Return: Comics/abc-123.jpg

            var uri = new Uri(url);
            return uri.AbsolutePath.TrimStart('/');
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

        public Task DeleteFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
