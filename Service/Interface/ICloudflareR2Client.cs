namespace ManhwaDimension.Service.Interface
{
    public interface ICloudflareR2Client
    {
        Task DeleteFileAsync(string v);
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
        Task DeleteComic(long id);
    }
}
