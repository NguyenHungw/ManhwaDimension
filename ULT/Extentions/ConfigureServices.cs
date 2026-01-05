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
            services.AddDbContext<BookwormDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DBConnection"),
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

            //services.AddScoped<IAccountMetaRepository, AccountMetaRepository>();
            //services.AddScoped<IAccountMetaService, AccountMetaService>();

            //services.AddScoped<IAccountStatusRepository, AccountStatusRepository>();
            //services.AddScoped<IAccountStatusService, AccountStatusService>();

            //services.AddScoped<IAccountTypeRepository, AccountTypeRepository>();
            //services.AddScoped<IAccountTypeService, AccountTypeService>();

            //services.AddScoped<IBiochemicalImportService , BiochemicalImportService>();

            //services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
            //services.AddScoped<IActivityLogService, ActivityLogService>();

            //services.AddScoped<IDistrictRepository, DistrictRepository>();
            //services.AddScoped<IDistrictService, DistrictService>();

            //services.AddScoped<IFolderUploadRepository, FolderUploadRepository>();

            //services.AddScoped<IMenuRepository, MenuRepository>();
            //services.AddScoped<IMenuService, MenuService>();

            //services.AddScoped<IMenuTypeRepository, MenuTypeRepository>();
            //services.AddScoped<IMenuTypeService, MenuTypeService>();

            //services.AddScoped<INotificationRepository, NotificationRepository>();
            //services.AddScoped<INotificationService, NotificationService>();

            //services.AddScoped<INotificationStatusRepository, NotificationStatusRepository>();
            //services.AddScoped<INotificationStatusService, NotificationStatusService>();

            //services.AddScoped<IPostRepository, PostRepository>();
            //services.AddScoped<IPostService, PostService>();

            //services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
            //services.AddScoped<IPostCategoryService, PostCategoryService>();

            //services.AddScoped<IPostStatusRepository, PostStatusRepository>();
            //services.AddScoped<IPostStatusService, PostStatusService>();

            //services.AddScoped<IPostTypeRepository, PostTypeRepository>();
            //services.AddScoped<IPostTypeService, PostTypeService>();

            //services.AddScoped<IProvinceRepository, ProvinceRepository>();
            //services.AddScoped<IProvinceService, ProvinceService>();

            //services.AddScoped<IRightsRepository, RightsRepository>();
            //services.AddScoped<IRightsService, RightsService>();

            //services.AddScoped<IRoleRepository, RoleRepository>();
            //services.AddScoped<IRoleService, RoleService>();

            //services.AddScoped<IRoleMenuRepository, RoleMenuRepository>();
            //services.AddScoped<IRoleMenuService, RoleMenuService>();

            //services.AddScoped<IRoleRightsRepository, RoleRightsRepository>();
            //services.AddScoped<IRoleRightsService, RoleRightsService>();

            //services.AddScoped<ISystemConfigRepository, SystemConfigRepository>();
            //services.AddScoped<ISystemConfigService, SystemConfigService>();

            //services.AddScoped<IUploadFileRepository, UploadFileRepository>();

            //services.AddScoped<IWardRepository, WardRepository>();
            //services.AddScoped<IWardService, WardService>();

            //services.AddScoped<IProfileService, ProfileService>();

            //services.AddScoped<IDiseaseDefinitionRepository, DiseaseDefinitionRepository>();
            //services.AddScoped<IDiseaseDefinitionService, DiseaseDefinitionService>();

            //services.AddScoped<IDiseaseScoreRuleRepository, DiseaseScoreRuleRepository>();
            //services.AddScoped<IDiseaseScoreRuleService, DiseaseScoreRuleService>();

            //services.AddScoped<IHospitalRepository, HospitalRepository>();
            //services.AddScoped<IHospitalService, HospitalService>();

            //services.AddScoped<IHospitalAccountRepository, HospitalAccountRepository>();
            //services.AddScoped<IHospitalAccountService, HospitalAccountService>();

            //services.AddScoped<IMetricRepository, MetricRepository>();
            //services.AddScoped<IMetricService, MetricService>();

            //services.AddScoped<IMetricDefinitionRepository, MetricDefinitionRepository>();
            //services.AddScoped<IMetricDefinitionService, MetricDefinitionService>();

            //services.AddScoped<IMetricThresholdRepository, MetricThresholdRepository>();
            //services.AddScoped<IMetricThresholdService, MetricThresholdService>();

            //services.AddScoped<IScoreRepository, ScoreRepository>();
            //services.AddScoped<IScoreService, ScoreService>();

            //services.AddScoped<IFormRequestRepository, FormRequestRepository>();
            //services.AddScoped<IFormRequestService, FormRequestService>();

            //services.AddScoped<IAdnInfoRepository, AdnInfoRepository>();
            //services.AddScoped<IAdnInfoService, AdnInfoService>();

            //services.AddScoped<IFamilyInfoRepository, FamilyInfoRepository>();
            //services.AddScoped<IFamilyInfoService, FamilyInfoService>();

            //services.AddScoped<ISampleRepository, SampleRepository>();
            //services.AddScoped<ISampleService, SampleService>();

            //services.AddScoped<IVariantRepository, VariantRepository>();
            //services.AddScoped<IVariantService, VariantService>();

            //services.AddScoped<IRelationshipRepository, RelationshipRepository>();
            //services.AddScoped<IRelationshipService, RelationshipService>();
            //#endregion

            //services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IEmailService, EmailService>();

            // Đăng ký HttpClient để gọi API bên ngoài
            services.AddHttpClient();

            return services;
        
        }
    }
}
