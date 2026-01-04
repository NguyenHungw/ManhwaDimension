using Microsoft.AspNetCore.Mvc;

namespace ManhwaDimension.Controllers.Core
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
