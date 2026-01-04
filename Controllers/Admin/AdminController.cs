using Microsoft.AspNetCore.Mvc;

namespace ManhwaDimension.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
