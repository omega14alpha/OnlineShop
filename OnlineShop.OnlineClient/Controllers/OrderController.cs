using Microsoft.Extensions.Logging;
using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;

namespace OnlineShop.OnlineClient.Controllers
{
    public class OrderController : DataTableController<OrderModel>
    {
        public OrderController(ILogger<OrderController> logger, IModelWorker<OrderModel> orderWorker) : base(logger, orderWorker)
        { }

    }
}
