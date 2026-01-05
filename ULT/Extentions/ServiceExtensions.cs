using ManhwaDimension.ULT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace ManhwaDimension.Util
{
    public static class ServiceExtensions
    {
        private static readonly HttpClient httpClient;
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {


            // Add services to the container.
            services.AddControllersWithViews().AddNewtonsoftJson().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            }).AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
            //services.AddSingleton<ICacheHelper, CacheHelper>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(72000);
            });
            services.AddSession(cfg =>
            {                            // Đăng ký dịch vụ Session
                cfg.Cookie.Name = SystemConstant.SECURITY_KEY_NAME; // "novaticSecurityToken";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 120, 0);           // Thời gian tồn tại của Session
            });
            services.AddControllersWithViews()
                .ConfigureApiBehaviorOptions(opt =>
                {
                    opt.SuppressModelStateInvalidFilter = true;
                })
                .AddSessionStateTempDataProvider();
            services.AddCors(options =>
            {
                options.AddPolicy("Default", policy =>
                {
                    policy.WithOrigins(configuration.GetSection("CorsOrigins").Get<string[]>())
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod();
                });
            });
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.FullName); // Đảm bảo schemaId là duy nhất bằng cách dùng FullName
                c.DescribeAllParametersInCamelCase();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    return apiDesc.RelativePath != null && apiDesc.RelativePath.ToLower().Contains("api");
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập 'Bearer <token>' vào ô dưới"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
                c.SchemaFilter<SwaggerFileSchemaFilter>();
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                    ValidAudience = configuration.GetValue<string>("Jwt:Issuer"),
                    IssuerSigningKey = new
                        SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes
                            (configuration.GetValue<string>("Jwt:Key")))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (!string.IsNullOrEmpty(context.Request.Cookies["Authorization"]))
                        {
                            context.Token = context.Request.Cookies["Authorization"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Bearer")
                    .Build();
            });
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            //HTTPS ENFORCE
            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
            });
            return services;

        }
        public static IServiceCollection AddInfrastructureQuazt(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddQuartz(options =>
            //{
            //    var jobKey = new JobKey(nameof(JobRepository));
            //    options.AddJob<JobRepository>(jobKey)
            //    .AddTrigger(
            //        trigger => trigger.ForJob(jobKey)
            //        .WithIdentity("TriggerCrawlDataEnv", "EnvGroup")
            //        .WithCronSchedule("0 30 6 * * ?")
            //        .StartNow()
            //        .WithPriority(1)
            //    );
            //    options.UseMicrosoftDependencyInjectionJobFactory();
            //});
            //services.AddQuartzHostedService(options =>
            //{
            //    options.WaitForJobsToComplete = true;
            //});
            return services;
        }
    }
    public class SwaggerFileSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(IFormFile))
            {
                schema.Type = "string";
                schema.Format = "binary";
            }
        }
    }
}
