using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.BusinessLogic;

namespace OnlineShop.OnlineClient.Controllers
{
    public class ChartController : Controller
    {
        private readonly ChartWorker _worker;

        public ChartController(ChartWorker worker)
        {
            _worker = worker;
        }

        [HttpGet]
        [Authorize(Roles = "user, admin")]
        public PartialViewResult Sales()
        {
            var result = _worker.GetSalesManagersSums();
            return PartialView(result);
        }
    }
}
