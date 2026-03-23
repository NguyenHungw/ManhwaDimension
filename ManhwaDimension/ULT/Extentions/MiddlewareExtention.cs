using ManhwaDimension.Constants;
using ManhwaDimension.Util;
using ManhwaDimension.Util.Extentions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class MiddlewareExtention
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    private static readonly HashSet<string> ClientPublicRoutes = new(StringComparer.OrdinalIgnoreCase)
    {
        "",
        "sign-in",
        "sign-up",
        "forgot-password",
        "reset-password",
        "client/sign-in",
        "client/sign-up"
    };

    private static readonly HashSet<string> AdminPublicRoutes = new(StringComparer.OrdinalIgnoreCase)
    {
        "admin",
        "admin/",
        "admin/action/sign-in",
        "admin/action/forgot-password",
        "admin/chinh-sach-bao-mat"
    };

    public MiddlewareExtention(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        using var scope = _serviceProvider.CreateScope();
        var authorizationService = scope.ServiceProvider.GetRequiredService<IAuthorizationService>();
        var path = context.Request.Path.Value?.TrimStart('/').ToLower() ?? string.Empty;

        if (IsStaticFile(path))
        {
            await _next(context);
            return;
        }

        if (path.Contains("api") || path.StartsWith("swagger"))
        {
            await _next(context);
            return;
        }

        if (IsAdminPath(path))
        {
            await HandleAdminRoutes(context, authorizationService, path);
            return;
        }

        await HandleClientRoutes(context, path);
    }

    private static bool IsStaticFile(string path)
    {
        return path.StartsWith("wwwroot") ||
               path.Contains(".css") ||
               path.Contains(".js") ||
               path.Contains(".png") ||
               path.Contains(".jpg") ||
               path.Contains(".jpeg") ||
               path.Contains(".gif") ||
               path.Contains(".svg") ||
               path.Contains(".ico") ||
               path.Contains(".woff") ||
               path.Contains(".woff2") ||
               path.Contains(".ttf") ||
               path.Contains(".eot");
    }

    private static bool IsAdminPath(string path)
    {
        return path.StartsWith("admin");
    }

    private async Task HandleAdminRoutes(HttpContext context, IAuthorizationService authorizationService, string path)
    {
        if (AdminPublicRoutes.Contains(path))
        {
            await _next(context);
            return;
        }

        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            context.Response.Redirect("/admin/action/sign-in");
            return;
        }

        var identity = context.User.Identity as ClaimsIdentity;
        var roleIdValue = identity?.FindFirst(ClaimNames.ROLE_ID)?.Value;

        if (!int.TryParse(roleIdValue, out var roleId))
        {
            context.Response.Redirect("/admin/action/sign-in");
            return;
        }

        //if (roleId == SystemConstant.ROLE_USER)
        //{
        //    context.Response.Redirect("/sign-in");
        //    return;
        //}

        //if (roleId == SystemConstant.ROLE_SYSTEM_ADMIN || roleId == SystemConstant.ROLE_SYSTEM_ADMIN_GS_1 || roleId == SystemConstant.ROLE_SYSTEM_ADMIN_GS_2 || roleId == SystemConstant.ROLE_HOSPITAL)
        //{
        //    await _next(context);
        //    return;
        //}

        //var authResult = await authorizationService.AuthorizeAsync(context.User, null, new RoleRequirement(path));
        //if (!authResult.Succeeded)
        //{
        //    context.Response.Redirect("/admin/action/sign-in");
        //    return;
        //}

        await _next(context);
    }

    private async Task HandleClientRoutes(HttpContext context, string path)
    {
        if (ClientPublicRoutes.Contains(path))
        {
            await _next(context);
            return;
        }

        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            context.Response.Redirect("/sign-in");
            return;
        }

        await _next(context);
    }
}
