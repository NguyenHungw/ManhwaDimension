namespace ManhwaDimension.Service.Interface
{
    public interface ICloudflareR2Client
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
    }
}
