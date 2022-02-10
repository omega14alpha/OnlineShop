using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;
using OnlineShop.OnlineClient.Infrastructure.Pagination;
using System;
using System.Collections.Generic;

namespace OnlineShop.OnlineClient.Controllers
{
    public abstract class DataTableController<TModel> : Controller
    {
        protected const int PageSize = 10;

        protected readonly ILogger _logger;

        protected readonly IModelWorker<TModel> _modelWorker;

        public DataTableController(ILogger logger, IModelWorker<TModel> modelWorker)
        {
            _logger = logger;
            _modelWorker = modelWorker;
        }

        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public PartialViewResult Index(int page = 1)
        {
            PaginationModel<TModel> viewModel = null;
            try
            {
                var models = _modelWorker.GetModels(page, PageSize, out int count);
                viewModel = CreatePagination(models, page, count);                
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

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

            if (string.IsNullOrWhiteSpace(filterModel.Data) && filterModel.Dates == null)
            {
                return PartialView("Index");
            }

            PaginationModel<TModel> viewModel = null;
            try
            {
                var models = _modelWorker.Filtration(1, PageSize, out int count, filterModel);
                viewModel = CreatePagination(models, 1, count);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

            return PartialView("Index", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public PartialViewResult Edit(int id)
        {
            try
            {
                return PartialView(_modelWorker.GetModel(1, id));                
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            
            return PartialView("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public PartialViewResult Save(TModel model)
        {
            if (model != null)
            {
                try
                {
                    _modelWorker.EditModel(model);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                }                
            }

            return Index();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            if (id >= 0)
            {
                try
                {
                    _modelWorker.DeleteModel(id);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                }
            }

            return Index();
        }

        protected PaginationModel<TModel> CreatePagination(IEnumerable<TModel> collection, int page, int count)
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
