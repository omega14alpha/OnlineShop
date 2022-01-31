using OnlineShop.BusinessLogic.Models;
using System.Collections.Generic;

namespace OnlineShop.BusinessLogic.Interfaces
{
    public interface IDbWorker
    {
        IEnumerable<OrderModel> GetOrders(int pageNumber, int totalSize, out int comonEntityCount);

        IEnumerable<ClientModel> GetClients(int pageNumber, int totalSize, out int comonEntityCount);

        IEnumerable<ManagerModel> GetManagers(int pageNumber, int totalSize, out int comonEntityCount);

        IEnumerable<ItemModel> GetItems(int pageNumber, int totalSize, out int comonEntityCount);
    }
}
