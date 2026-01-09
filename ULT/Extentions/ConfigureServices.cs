using Amazon;
using Amazon.S3;
using ManhwaDimension;
using ManhwaDimension.Models;
using ManhwaDimension.Repository;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Service;
using ManhwaDimension.Service.Interface;
using ManhwaDimension.ULT.Extentions;
using ManhwaDimension.Util.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace ManhwaDimension.Util
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookwormDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(BookwormDbContext).Assembly.FullName)));
           
            //var region = configuration["AWS:Region"];  // Đọc vùng từ cấu hình
            //var s3Client = new AmazonS3Client(RegionEndpoint.USEast1);  // Khởi tạo trực tiếp bằng RegionEndpoint nếu GetBySystemName không hoạt động
            //Console.WriteLine(configuration["AWS:Region"]);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            //services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();
            #region UploadFile
            //services.AddScoped<IFileExplorerService, FileExplorerService>();
            //services.AddScoped<IFileStorageService, FileStorageService>();
            //services.AddScoped<IUploadFileRepository, UploadFileRepository>();
            //services.AddScoped<IFolderUploadRepository, FolderUploadRepository>();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            #endregion
            // #region Add scope service and repository
            //services.AddScoped<IAccountRepository, AccountRepository>();
            //services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IGenreService, GenreService> ();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IAuthorService, AuthorService>();    
            services.AddScoped<IComicRepository, ComicRepository>();    
            services.AddScoped<IComicService, ComicService>();

            services.Configure<CloudflareR2ClientOptions>(configuration.GetSection("CloudflareR2"));
            services.AddScoped<ICloudflareR2Client>(serviceProvider =>
            {
                // Lấy thông tin cấu hình từ phần "CloudflareR2" trong appsettings.json
               // var awsConfig = configuration.GetSection("AWS");
                var awsConfig = configuration.GetSection("AWS");
                var cloudflareR2Config = configuration.GetSection("CloudflareR2");


                var accessKey = cloudflareR2Config["AccessKey"];
                var secretKey = cloudflareR2Config["SecretKey"];
                var bucketName = cloudflareR2Config["BucketName"];
                var endpoint = cloudflareR2Config["Endpoint"];
                var awsKey = awsConfig["AWSAccessKey"];
                var awsSecret = awsConfig["AWSSecretKey"];

                // Tạo đối tượng CloudflareR2Client với các tham số cấu hình
                return new CloudflareR2Client(accessKey, secretKey, bucketName, endpoint, awsKey , awsSecret);
            });

            // Đăng ký HttpClient để gọi API bên ngoài
            services.AddHttpClient();

            return services;
        
        }
    }
}
