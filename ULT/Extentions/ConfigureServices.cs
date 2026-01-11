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

            //services.Configure<CloudflareR2ClientOptions>(configuration.GetSection("CloudflareR2"));
            // Program.cs (.NET 6+)
            services.AddSingleton<ICloudflareR2Client>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();

                var accessKey = config["CloudflareR2:AccessKey"];
                var secretKey = config["CloudflareR2:SecretKey"];
                var bucketName = config["CloudflareR2:BucketName"];
                var endpoint = config["CloudflareR2:Endpoint"];
                var accountId = config["CloudflareR2:AccountId"];

                return new CloudflareR2Client(accessKey, secretKey, bucketName, endpoint, accountId);
            });

            // Đăng ký HttpClient để gọi API bên ngoài
            services.AddHttpClient();

            return services;
        
        }
    }
}
