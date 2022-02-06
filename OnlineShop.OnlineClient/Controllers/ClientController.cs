using Microsoft.Extensions.Logging;
using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;

namespace OnlineShop.OnlineClient.Controllers
{
    public class ClientController : DataTableController<ClientModel>
    {
        public ClientController(ILogger<ClientController> logger, IModelWorker<ClientModel> clientWorker) : base(logger, clientWorker)
        { }

    }
}
