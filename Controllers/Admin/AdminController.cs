using Microsoft.AspNetCore.Mvc;

namespace ManhwaDimension.Controllers.Admin
{
    [Route("admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("dashboard")]
        public async Task<IActionResult> DashBoard()
        {
            return View();
        }
    }
}
