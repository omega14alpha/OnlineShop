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

        [HttpGet]
        public PartialViewResult MonthDropDownMenu()
        {
            var managers = _worker.GetManagerList();
            return PartialView("MonthDropDownMenu", managers);
        }

        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public IActionResult MonthSales(string manager)
        {
            var result = _worker.GetMonthSalesManagers(manager);
            return PartialView("MonthSales", result);
        }
    }
}
