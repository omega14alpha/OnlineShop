using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;
using OnlineShop.OnlineClient.Pagination.Models;
using System.Collections.Generic;

namespace OnlineShop.OnlineClient.Controllers
{
    public class ManagerController : Controller
    {
        private const int PageSize = 10;

        private readonly ILogger<ManagerController> _logger;

        private readonly IModelWorker<ManagerModel> _managerWorker;

        public ManagerController(ILogger<ManagerController> logger, IModelWorker<ManagerModel> managerWorker)
        {
            _logger = logger;
            _managerWorker = managerWorker;
        }

        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public IActionResult Index(int page = 1)
        {
            var managers = _managerWorker.GetModels(page, PageSize, out int count);
            var viewModel = CreatePagination(managers, page, count);
            return PartialView(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var manager = _managerWorker.GetModel(1, id);
            if (manager != null)
            {
                return View(manager);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Save(ManagerModel model)
        {
            if (model != null)
            {
                _managerWorker.EditModel(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            _managerWorker.DeleteModel(id);
            return View("Index");
        }

        private PaginationModel<ManagerModel> CreatePagination(IEnumerable<ManagerModel> collection, int page, int count)
        {
            return new PaginationModel<ManagerModel>()
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
