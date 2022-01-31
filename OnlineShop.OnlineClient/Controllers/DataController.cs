using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;
using OnlineShop.OnlineClient.Pagination.Models;
using System.Collections.Generic;

namespace OnlineShop.OnlineClient.Controllers
{
    public class DataController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IDbWorker _worker;

        private const int PageSize = 10;

        public DataController(ILogger<HomeController> logger, IDbWorker worker)
        {
            _logger = logger;
            _worker = worker;
        }

        public IActionResult DataPage()
        {
            return View();
        }

        //  [Authorize(Roles = "user, admin")]
        public IActionResult Orders(int page = 1)
        {
            var orders = _worker.GetOrders(page, PageSize, out int count);
            var viewModel = CreatePagination(orders, page, count);
            return View(viewModel);
        }

        //  [Authorize(Roles = "user, admin")]
        public IActionResult Clients(int page = 1)
        {
            var clients = _worker.GetClients(page, PageSize, out int count);
            var viewModel = CreatePagination(clients, page, count);
            return View(viewModel);
        }

        //  [Authorize(Roles = "user, admin")]
        public IActionResult Items(int page = 1)
        {
            var items = _worker.GetItems(page, PageSize, out int count);
            var viewModel = CreatePagination(items, page, count);
            return View(viewModel);
        }

        //  [Authorize(Roles = "user, admin")]
        public IActionResult Managers(int page = 1)
        {
            var managers = _worker.GetManagers(page, PageSize, out int count);
            var viewModel = CreatePagination(managers, page, count);
            return View(viewModel);
        }

        public IActionResult AlterOrder(OrderModel model)
        {

            return View();
        }

        private PaginationModel<T> CreatePagination<T>(IEnumerable<T> collection, int page, int count)
        {
            return new PaginationModel<T>()
            {
                Items = collection,
                PageInfo = new PageInfoModel()
                {
                    PageNumber = page,
                    PageSize = PageSize,
                    TotalItems = count
                }
            };
        }
    }
}
