using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT.Extentions;
namespace ManhwaDimension.Service
{
    public class CloudflareR2Client : ICloudflareR2Client
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _bucketName;
        private readonly string _endpoint;
        private readonly string _awskey;
        private readonly string _awsaccess; 
        private readonly ULT.Extentions.CloudflareR2ClientOptions _options;
        private readonly AwsClientOptions _awsClientOptions;
        public CloudflareR2Client(string accesskey, string secretKey, string bucketName, string endpoint, string awskey , string awsaccess)
        {
            _accessKey = accesskey;
            _secretKey = secretKey;
            _bucketName = bucketName;
            _endpoint = endpoint;
            _accessKey = awskey;
            _secretKey = awsaccess;
        }
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var credentials = new BasicAWSCredentials(_accessKey, _secretKey);
            var config = new AmazonS3Config
            {
                ServiceURL = _endpoint,
                ForcePathStyle = true, // Important for R2 to work correctly
                RegionEndpoint = Amazon.RegionEndpoint.USEast1, // You can choose any region
            };
            // Tạo kết nối với Cloudflare R2 và upload file
 
            var client = new AmazonS3Client(credentials, config);

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream
            };

            await client.PutObjectAsync(putRequest);


            // Trả về URL của ảnh đã upload
            return $"https://{_bucketName}.{_endpoint}/{fileName}";
        }
    }
}
