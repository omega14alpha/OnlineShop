using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OnlineShop.OnlineClient.Controllers
{
    public class DataController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public DataController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult DataPage()
        {
            return View();
        }
    }
}
