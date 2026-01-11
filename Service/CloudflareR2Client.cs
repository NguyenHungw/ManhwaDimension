using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using ManhwaDimension.Service.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ManhwaDimension.Service
{
    public class CloudflareR2Client : ICloudflareR2Client
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _bucketName;
        private readonly string _endpoint;
        private readonly string _accountId;
        private readonly AmazonS3Client _s3Client;

        public CloudflareR2Client(string accessKey, string secretKey, string bucketName, string endpoint, string accountId)
        {
            if (string.IsNullOrEmpty(accessKey)) throw new ArgumentNullException(nameof(accessKey));
            if (string.IsNullOrEmpty(secretKey)) throw new ArgumentNullException(nameof(secretKey));
            if (string.IsNullOrEmpty(bucketName)) throw new ArgumentNullException(nameof(bucketName));
            if (string.IsNullOrEmpty(endpoint)) throw new ArgumentNullException(nameof(endpoint));
            if (string.IsNullOrEmpty(accountId)) throw new ArgumentNullException(nameof(accountId));

            _accessKey = accessKey;
            _secretKey = secretKey;
            _bucketName = bucketName;
            _endpoint = endpoint;
            _accountId = accountId;

            // Cấu hình cho Cloudflare R2
            var config = new AmazonS3Config
            {
                ServiceURL = _endpoint,
                ForcePathStyle = true,
                AuthenticationRegion = "auto"
                // Bỏ SignatureVersion vì không còn support
                // SignatureVersion sẽ tự động là v4
            };

            var credentials = new BasicAWSCredentials(_accessKey, _secretKey);
            _s3Client = new AmazonS3Client(credentials, config);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            try
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = fileStream,
                    // QUAN TRỌNG: Disable chunked encoding
                    DisablePayloadSigning = true,
                    //UseChunkEncoding = false,
                    AutoCloseStream = false,
                    CannedACL = S3CannedACL.PublicRead


                };

                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);

                // URL đúng cho R2 public bucket
                return $"https://pub-{_accountId}.r2.dev/{fileName}";
            }
            catch (AmazonS3Exception e)
            {
                throw new Exception($"Error uploading file to R2: {e.Message}", e);
            }
        }

        // Method test connection
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await _s3Client.ListBucketsAsync();
                Console.WriteLine($"Connected successfully! Found {response.Buckets.Count} buckets");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection test failed: {ex.Message}");
                return false;
            }
        }
    }
}