using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;
using OnlineShop.OnlineClient.Pagination.Models;
using System.Collections.Generic;

namespace OnlineShop.OnlineClient.Controllers
{
    public abstract class DataTableController<TModel> : Controller
    {
        private const int PageSize = 10;

        private readonly ILogger _logger;

        private readonly IModelWorker<TModel> _modelWorker;

        public DataTableController(ILogger logger, IModelWorker<TModel> modelWorker)
        {
            _logger = logger;
            _modelWorker = modelWorker;
        }

        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public IActionResult Index(int page = 1)
        {
            var orders = _modelWorker.GetModels(page, PageSize, out int count);
            var viewModel = CreatePagination(orders, page, count);
            return PartialView(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "user, admin")]
        public PartialViewResult Filtration()
        {
            return PartialView();
        }

        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public IActionResult Search(FilterDataModel filterModel)
        {
            if (filterModel == null)
            {
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(filterModel.Data))
            {
                return PartialView("Index");
            }

            var items = _modelWorker.Filtration(1, PageSize, out int count, filterModel);
            var viewModel = CreatePagination(items, 1, count);
            return PartialView("Index", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public PartialViewResult Edit(int id)
        {
            var order = _modelWorker.GetModel(1, id);
            if (order != null)
            {
                return PartialView(order);
            }
            
            return PartialView("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Save(TModel model)
        {
            if (model != null)
            {
                _modelWorker.EditModel(model);
            }

            return PartialView("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            _modelWorker.DeleteModel(id);
            return View("Index");
        }

        private PaginationModel<TModel> CreatePagination(IEnumerable<TModel> collection, int page, int count)
        {
            return new PaginationModel<TModel>()
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
