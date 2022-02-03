using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;
using OnlineShop.OnlineClient.Pagination.Models;
using System.Collections.Generic;

namespace OnlineShop.OnlineClient.Controllers
{
    public class ClientController : Controller
    {
        private const int PageSize = 10;

        private readonly ILogger<ClientController> _logger;

        private readonly IModelWorker<ClientModel> _clientWorker;

        public ClientController(ILogger<ClientController> logger, IModelWorker<ClientModel> clientWorker)
        {
            _logger = logger;
            _clientWorker = clientWorker;
        }

        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public IActionResult Index(int page = 1)
        {
            var clients = _clientWorker.GetModels(page, PageSize, out int count);
            var viewModel = CreatePagination(clients, page, count);
            return PartialView(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var client = _clientWorker.GetModel(1, id);
            if (client != null)
            {
                return View(client);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Save(ClientModel model)
        {
            if (model != null)
            {
                _clientWorker.EditModel(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            _clientWorker.DeleteModel(id);
            return View("Index");
        }

        private PaginationModel<ClientModel> CreatePagination(IEnumerable<ClientModel> collection, int page, int count)
        {
            return new PaginationModel<ClientModel>()
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
