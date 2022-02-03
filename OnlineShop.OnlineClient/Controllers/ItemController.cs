using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;
using OnlineShop.OnlineClient.Pagination.Models;
using System.Collections.Generic;

namespace OnlineShop.OnlineClient.Controllers
{
    public class ItemController : Controller
    {
        private const int PageSize = 10;

        private readonly ILogger<ItemController> _logger;

        private readonly IModelWorker<ItemModel> _itemWorker;

        public ItemController(ILogger<ItemController> logger, IModelWorker<ItemModel> itemWorker)
        {
            _logger = logger;
            _itemWorker = itemWorker;
        }

        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public IActionResult Index(int page = 1)
        {
            var items = _itemWorker.GetModels(page, PageSize, out int count);
            var viewModel = CreatePagination(items, page, count);
            return PartialView(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var item = _itemWorker.GetModel(1, id);
            if (item != null)
            {
                return View(item);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Save(ItemModel model)
        {
            if (model != null)
            {
                _itemWorker.EditModel(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            _itemWorker.DeleteModel(id);
            return View("Index");
        }

        private PaginationModel<ItemModel> CreatePagination(IEnumerable<ItemModel> collection, int page, int count)
        {
            return new PaginationModel<ItemModel>()
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
