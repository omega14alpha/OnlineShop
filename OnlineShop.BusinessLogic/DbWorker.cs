using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;
using OnlineShop.DataAccess.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.BusinessLogic
{
    public  class DbWorker : IDbWorker
    {
        private const string DatePattern = "dd.MM.yyyy";

        private readonly DbOrderContext _dbContext;

        public DbWorker(DbOrderContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<OrderModel> GetOrders(int pageNumber, int totalSize, out int comonEntityCount)
        {
            comonEntityCount = _dbContext.Orders.Count();
            var orderModels = new List<OrderModel>();
            var orders = _dbContext.Orders.OrderBy(s => s.Date).Skip((pageNumber - 1) * totalSize).Take(totalSize).ToList();
            var id = (pageNumber - 1) * totalSize;
            foreach (var order in orders)
            {
                var newOrder = new OrderModel()
                {
                    Id = id++,
                    OrderId = order.Id,
                    Date = order.Date.ToString(DatePattern),
                    Manager = _dbContext.Managers?.FirstOrDefault(m => m.Id == order.ManagerId).Surname,
                    Client = _dbContext.Clients?.FirstOrDefault(c => c.Id == order.ClientId).Name,
                    Item = _dbContext.Items?.FirstOrDefault(i => i.Id == order.ItemId).Name,
                    AmountOfMoney = order.AmountOfMoney
                };

                orderModels.Add(newOrder);
            }            

            return orderModels;
        }

        public IEnumerable<ClientModel> GetClients(int pageNumber, int totalSize, out int comonEntityCount)
        {
            comonEntityCount = _dbContext.Clients.Count();
            var clientModels = new List<ClientModel>();
            var clients = _dbContext.Clients.Skip((pageNumber - 1) * totalSize).Take(totalSize).ToList();
            var id = (pageNumber - 1) * totalSize;
            foreach (var client in clients)
            {
                var newClien = new ClientModel()
                {
                    Id = id++,
                    Name = client.Name
                };

                clientModels.Add(newClien);
            }

            return clientModels;
        }

        public IEnumerable<ItemModel> GetItems(int pageNumber, int totalSize, out int comonEntityCount)
        {
            comonEntityCount = _dbContext.Orders.Count();
            var itemModels = new List<ItemModel>();
            var items = _dbContext.Items.Skip((pageNumber - 1) * totalSize).Take(totalSize).ToList();
            var id = (pageNumber - 1) * totalSize;
            foreach (var item in items)
            {
                var newItem = new ItemModel()
                {
                    Id = id++,
                    Name = item.Name
                };

                itemModels.Add(newItem);
            }

            return itemModels;
        }

        public IEnumerable<ManagerModel> GetManagers(int pageNumber, int totalSize, out int comonEntityCount)
        {
            comonEntityCount = _dbContext.Orders.Count();
            var managerModels = new List<ManagerModel>();
            var managers = _dbContext.Managers.Skip((pageNumber - 1) * totalSize).Take(totalSize).ToList();
            var id = (pageNumber - 1) * totalSize;
            foreach (var manager in managers)
            {
                var newManager = new ManagerModel()
                {
                    Id = id++,
                    Surname = manager.Surname
                };

                managerModels.Add(newManager);
            }

            return managerModels;
        }
    }
}
