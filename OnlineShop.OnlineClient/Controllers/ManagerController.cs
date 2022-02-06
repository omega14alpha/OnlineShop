using Microsoft.Extensions.Logging;
using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;

namespace OnlineShop.OnlineClient.Controllers
{
    public class ManagerController : DataTableController<ManagerModel>
    {
        public ManagerController(ILogger<ManagerController> logger, IModelWorker<ManagerModel> managerWorker) : base(logger, managerWorker)
        { }

    }
}
