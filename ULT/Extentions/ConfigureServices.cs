using ManhwaDimension.Models;
using ManhwaDimension.Repository;
using ManhwaDimension.Repository.Interface;
using ManhwaDimension.Service;
using ManhwaDimension.Service.Interface;
using ManhwaDimension;

using ManhwaDimension.Util.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace ManhwaDimension.Util
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookwormDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(BookwormDbContext).Assembly.FullName)));

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

            // Đăng ký HttpClient để gọi API bên ngoài
            services.AddHttpClient();

            return services;
        
        }
    }
}
