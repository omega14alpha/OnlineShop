using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;
using OnlineShop.OnlineClient.Pagination.Models;
using System.Collections.Generic;

namespace OnlineShop.OnlineClient.Controllers
{
    public class OrderController : Controller
    {
        private const int PageSize = 10;

        private readonly ILogger<OrderController> _logger;

        private readonly IModelWorker<OrderModel> _orderWorker;

        public OrderController(ILogger<OrderController> logger, IModelWorker<OrderModel> orderWorker)
        {
            _logger = logger;
            _orderWorker = orderWorker;
        }

        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public IActionResult Index(int page = 1)
        {
            var orders = _orderWorker.GetModels(page, PageSize, out int count);
            var viewModel = CreatePagination(orders, page, count);
            return PartialView(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var order = _orderWorker.GetModel(1, id);
            if (order != null)
            {
                return View(order);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Save(OrderModel model)
        {
            if (model != null)
            {
                _orderWorker.EditModel(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            _orderWorker.DeleteModel(id);
            return View("Index");
        }

        private PaginationModel<OrderModel> CreatePagination(IEnumerable<OrderModel> collection, int page, int count)
        {
            return new PaginationModel<OrderModel>()
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
