using Microsoft.Extensions.Logging;
using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;

namespace OnlineShop.OnlineClient.Controllers
{
    public class ItemController : DataTableController<ItemModel>
    {
        public ItemController(ILogger<ItemController> logger, IModelWorker<ItemModel> itemWorker) : base(logger, itemWorker)
        { }

    }
}
