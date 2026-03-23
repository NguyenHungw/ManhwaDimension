using ManhwaDimension.Constants;
using ManhwaDimension.Models;
using ManhwaDimension.Repository;
using ManhwaDimension.Util;
using System.Security.Claims;

public class MenuMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public MenuMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    //public async Task Invoke(HttpContext context)
    //{
    //    var path = context.Request.Path.Value?.TrimStart('/').ToLower() ?? "";

    //    // 🔹 Chỉ xử lý menu cho admin routes
    //    // Client có menu tĩnh, không cần load từ RoleMenu
    //    if (!path.Contains("admin"))
    //    {
    //        // Với client routes, chỉ lưu thông tin cơ bản
    //        await HandleClientContext(context);
    //        await _next(context);
    //        return;
    //    }

    //    // 🔹 Xử lý menu cho admin routes
    //    await HandleAdminMenu(context);
    //    await _next(context);
    //}

    /// <summary>
    /// Xử lý context cho Client routes
    /// </summary>
    //private async Task HandleClientContext(HttpContext context)
    //{
    //    using (var scope = _serviceProvider.CreateScope())
    //    {
    //        var _roleMenuRepository = scope.ServiceProvider.GetRequiredService<IRoleMenuRepository>();
    //        var _notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
    //        var _hospitalRepository = scope.ServiceProvider.GetRequiredService<IHospitalRepository>();

    //        int accountId = 0;
    //        int roleId = 0;
    //        var identity = context.User.Identity as ClaimsIdentity;

    //        if (identity != null)
    //        {
    //            var accountClaim = identity.FindFirst(ClaimNames.ID);
    //            if (accountClaim != null && int.TryParse(accountClaim.Value, out accountId))
    //            {
    //                context.Items["ClientAccountId"] = accountId;
    //            }

    //            var roleClaim = identity.FindFirst(ClaimNames.ROLE_ID);
    //            if (roleClaim != null && int.TryParse(roleClaim.Value, out roleId))
    //            {
    //                context.Items["ClientRoleId"] = roleId;
    //            }
    //        }
    //        if (accountId > 0)
    //        {
    //            var notifications = await _notificationRepository.ListNotificationByAccountIdPaging(accountId, 1, 10);
    //            var countNotification = await _notificationRepository.CountNotification(accountId);
    //            context.Items["MyNotification"] = notifications.ToList();
    //            context.Items["CountNotification"] = countNotification;

    //            //Loading page config
    //            //Nếu là role bệnh viện thì load config Hospital ra
    //            if (roleId == SystemConstant.ROLE_HOSPITAL)
    //            {
    //                var hospitalConfigs = await _hospitalRepository.GetByAccount(accountId);
    //                context.Items["HospitalConfig"] = hospitalConfigs;
    //            }
    //        }
           
    //    }
    //}

    /// <summary>
    /// Xử lý menu cho Admin routes
    /// </summary>
    //private async Task HandleAdminMenu(HttpContext context)
    //{
    //    using (var scope = _serviceProvider.CreateScope())
    //    {
    //        var _roleMenuRepository = scope.ServiceProvider.GetRequiredService<IRoleMenuRepository>();
    //        var _notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
    //        var _hospitalRepository = scope.ServiceProvider.GetRequiredService<IHospitalRepository>();

    //        int roleId = 0;
    //        int accountId = 0;
    //        var identity = context.User.Identity as ClaimsIdentity;

    //        if (identity != null)
    //        {
    //            var roleClaim = identity.FindFirst(ClaimNames.ROLE_ID);
    //            if (roleClaim != null)
    //            {
    //                int.TryParse(roleClaim.Value, out roleId);
    //            }

    //            var accountClaim = identity.FindFirst(ClaimNames.ID);
    //            if (accountClaim != null)
    //            {
    //                int.TryParse(accountClaim.Value, out accountId);
    //            }
    //        }

    //        // 🔹 Không load menu cho ROLE_USER (client user) khi truy cập admin routes
    //        if (roleId > 0 && roleId != SystemConstant.ROLE_USER)
    //        {
    //            var roleMenus = await _roleMenuRepository.ListByRole(roleId);
    //            var menuList = roleMenus
    //                .Where(x => x.Menu.MenuTypeId == 1001)
    //                .Select(x => x.Menu)
    //                .OrderBy(x => x.Priority)
    //                .ToList();

    //            context.Items["UserMenu"] = menuList;

    //            // Tìm URL hiện tại
    //            var requestedUrl = context.Request.Path.Value?.ToLower().Replace("/", "") ?? "";

    //            // Tìm breadcrumb theo URL
    //            var breadcrumb = menuList.FirstOrDefault(x =>
    //                (x.Url ?? "").ToLower().Replace("/", "") == requestedUrl
    //            );

    //            if (breadcrumb != null)
    //            {
    //                context.Items["BreadcrumbName"] = breadcrumb.Name;
    //            }
    //        }

    //        // Load notifications cho admin nếu cần
    //        if (accountId > 0)
    //        {
    //            var notifications = await _notificationRepository.ListNotificationByAccountIdPaging(accountId, 1, 10);
    //            var countNotification = await _notificationRepository.CountNotification(accountId);
    //            context.Items["MyNotification"] = notifications.ToList();
    //            context.Items["CountNotification"] = countNotification;


    //            //Loading page config
    //            //Nếu là role bệnh viện thì load config Hospital ra
    //            if (roleId == SystemConstant.ROLE_HOSPITAL)
    //            {
    //                var hospitalConfigs = await _hospitalRepository.GetByAccount(accountId);
    //                context.Items["HospitalConfig"] = hospitalConfigs;
    //            }
    //        }
    //    }
    //}
}
