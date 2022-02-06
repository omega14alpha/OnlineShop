using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic.Models;
using OnlineShop.DataAccess;
using OnlineShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OnlineShop.BusinessLogic
{
    public class OrderWorker : IModelWorker<OrderModel>
    {
        private const string DatePattern = "dd.MM.yyyy";

        private readonly DataBaseUoW _dbUoW;

        public OrderWorker(DataBaseUoW dataBaseUoW)
        {
            _dbUoW = dataBaseUoW;
        }

        public IEnumerable<OrderModel> GetModels(int pageNumber, int totalSize, out int comonEntityCount)
        {
            comonEntityCount = _dbUoW.Orders.GetCount();
            var orderModels = new List<OrderModel>();
            var orders = _dbUoW.Orders.GetRangeWithOrder((pageNumber - 1) * totalSize, totalSize, o => o.Date);
            var displayedId = (pageNumber - 1) * totalSize + 1;
            foreach (var order in orders)
            {
                orderModels.Add(EntityToModel(order, displayedId));
                displayedId++;
            }

            return orderModels;
        }

        public OrderModel GetModel(int displayedId, int modelId)
        {
            var order = _dbUoW.Orders.GetEntityByCondition(o => o.Id == modelId);
            return EntityToModel(order, displayedId);
        }

        public void EditModel(OrderModel model)
        {
            var order = ModelToEntity(model);
            _dbUoW.Orders.Update(order);
            _dbUoW.Save();
        }

        public void DeleteModel(int id)
        {
            _dbUoW.Orders.Remove(id);
        }

        public IEnumerable<OrderModel> Filtration(int pageNumber, int totalSize, out int comonEntityCount, FilterDataModel filterModel)
        {            
            comonEntityCount = _dbUoW.Orders.GetCount();
            var orderModels = new List<OrderModel>();
           /* var orders = _dbUoW.Orders.GetRangeByCondition((pageNumber - 1) * totalSize, totalSize, s => s.Name.StartsWith(filterModel.Data));
            var displayedId = (pageNumber - 1) * totalSize + 1;
            foreach (var order in orders)
            {
                orderModels.Add(EntityToModel(order, displayedId));
                displayedId++;
            }*/

            return orderModels;
        }

        private OrderModel EntityToModel(Order order, int displayedId)
        {
            var managerName = _dbUoW.Managers.GetEntityByCondition(x => x.Id == order.ManagerId).Surname;
            var clientName = _dbUoW.Clients.GetEntityByCondition(x => x.Id == order.ClientId).Name;
            var itemName = _dbUoW.Items.GetEntityByCondition(x => x.Id == order.ItemId).Name;

            return new OrderModel()
            {
                Id = displayedId,
                OrderId = order.Id,
                Date = order.Date.ToString(DatePattern),
                Manager = managerName,
                Client = clientName,
                Item = itemName,
                AmountOfMoney = order.AmountOfMoney
            };
        }

        private Order ModelToEntity(OrderModel model)
        {
            var manager = _dbUoW.Managers.GetOrAddEntity(new Manager() { Surname = model.Manager }, s => s.Surname == model.Manager);
            var client = _dbUoW.Clients.GetOrAddEntity(new Client() { Name = model.Client }, s => s.Name == model.Client);
            var item = _dbUoW.Items.GetOrAddEntity(new Item() { Name = model.Item }, s => s.Name == model.Item);
            var order = _dbUoW.Orders.GetEntityByCondition(i => i.Id == model.OrderId);

            if (order != null)
            {
                order.Date = DateTime.ParseExact(model.Date, DatePattern, CultureInfo.InvariantCulture);
                order.AmountOfMoney = model.AmountOfMoney;
                order.Manager = manager;
                order.Client = client;
                order.Item = item;
            }

            return order;
        }
    }
}
