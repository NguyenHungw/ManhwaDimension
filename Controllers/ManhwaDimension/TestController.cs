using ManhwaDimension.Service;
using ManhwaDimension.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ManhwaDimension.Controllers.ManhwaDimension
{
    public class TestController : Controller
    {
        private readonly ICloudflareR2Client _r2Client;

        public TestController(ICloudflareR2Client r2Client)
        {
            _r2Client = r2Client;
        }

        [Route("api/manhwadimension/test-r2")]
        [HttpGet]
        public async Task<IActionResult> TestR2Connection()
        {
            var isConnected = await ((CloudflareR2Client)_r2Client).TestConnectionAsync();
            return Ok(new { connected = isConnected });
        }

    }
}
