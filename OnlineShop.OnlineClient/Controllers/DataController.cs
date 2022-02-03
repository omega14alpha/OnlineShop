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
            var qwe = User.IsInRole("admin");
            var zxc = User.IsInRole("user");

            return View();
        }
    }
}
