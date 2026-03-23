using Microsoft.AspNetCore.Mvc;

namespace ManhwaDimension.Controllers.Client
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
