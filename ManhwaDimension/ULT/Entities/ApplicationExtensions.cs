using ManhwaDimension.Constants;
using System.Security.Claims;

namespace ManhwaDimension.Util.Entities
{
    public static class ApplicationExtensions
    {
        public static async void UseInfrastructure(this WebApplication app)
        {
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("Default");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<MiddlewareExtention>();
            app.UseMiddleware<MenuMiddleware>();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
                );
        }
    }
}
