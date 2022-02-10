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
            _dbUoW.Save();
        }

        public IEnumerable<OrderModel> Filtration(int pageNumber, int totalSize, out int comonEntityCount, FilterDataModel filterModel)
        {           

            var orderModels = new List<OrderModel>();
            var orders = GetPerception((pageNumber - 1) * totalSize, totalSize, out comonEntityCount, filterModel);
            if (orders != null)
            {
                var displayedId = (pageNumber - 1) * totalSize + 1;
                foreach (var order in orders)
                {
                    orderModels.Add(EntityToModel(order, displayedId));
                    displayedId++;
                }
            }

            return orderModels;
        }
                
        private IEnumerable<Order> GetPerception(int startNumber, int count, out int comonEntityCount, FilterDataModel filterModel) =>
            filterModel.Field switch
            {
                "Manager" => FiltrationByManager(startNumber, count, out comonEntityCount, filterModel.Data),
                "Client" => FiltrationByClient(startNumber, count, out comonEntityCount, filterModel.Data),
                "Item" => FiltrationByItem(startNumber, count, out comonEntityCount, filterModel.Data),
                "AmountOfMoney" => FiltrationByPrice(startNumber, count, out comonEntityCount, filterModel.Data),
                "Date" => FiltrationByDate(startNumber, count, out comonEntityCount, filterModel.Dates),
                _ => throw new ArgumentException()
            };
        

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

        private IEnumerable<Order> FiltrationByManager(int startNumber, int count, out int comonEntityCount, string strPart)
        {
            var entityId = _dbUoW.Managers.GetEntityByCondition(s => s.Surname.StartsWith(strPart, StringComparison.InvariantCultureIgnoreCase)).Id;
            comonEntityCount = _dbUoW.Orders.GetCountByCondition(s => s.ManagerId == entityId);
            return _dbUoW.Orders.GetRangeByConditionWithOrder(startNumber, count, s => s.ManagerId == entityId, d => d.Date);
        }

        private IEnumerable<Order> FiltrationByClient(int startNumber, int count, out int comonEntityCount, string strPart)
        {
            var entityId = _dbUoW.Clients.GetEntityByCondition(s => s.Name.StartsWith(strPart, StringComparison.InvariantCultureIgnoreCase)).Id;
            comonEntityCount = _dbUoW.Orders.GetCountByCondition(s => s.ClientId == entityId);
            return _dbUoW.Orders.GetRangeByConditionWithOrder(startNumber, count, s => s.ClientId == entityId, d => d.Date);
        }

        private IEnumerable<Order> FiltrationByItem(int startNumber, int count, out int comonEntityCount, string strPart)
        {
            var entityId = _dbUoW.Items.GetEntityByCondition(s => s.Name.StartsWith(strPart, StringComparison.InvariantCultureIgnoreCase)).Id;
            comonEntityCount = _dbUoW.Orders.GetCountByCondition(s => s.ItemId == entityId);
            return _dbUoW.Orders.GetRangeByConditionWithOrder(startNumber, count, s => s.ItemId == entityId, d => d.Date);
        }

        private IEnumerable<Order> FiltrationByPrice(int startNumber, int count, out int comonEntityCount, string strPart)
        {
            if (double.TryParse(strPart, out var number))
            {
                comonEntityCount = _dbUoW.Orders.GetCountByCondition(s => s.AmountOfMoney == number);
                return _dbUoW.Orders.GetRangeByConditionWithOrder(startNumber, count, s => s.AmountOfMoney == number, d => d.Date);
            }

            comonEntityCount = 0;
            return null;
        }

        private IEnumerable<Order> FiltrationByDate(int startNumber, int count, out int comonEntityCount, DateModel dates)
        {
            if (!string.IsNullOrWhiteSpace(dates.DateFrom) && !string.IsNullOrWhiteSpace(dates.DateTo))
            {
                return FiltrationByDateRange(startNumber, count, out comonEntityCount, dates);
            }
            else if (!string.IsNullOrWhiteSpace(dates.DateFrom))
            {
                return FiltrationByDateMoreThen(startNumber, count, out comonEntityCount, dates);
            }
            else
            {
                return FiltrationByDateLessThen(startNumber, count, out comonEntityCount, dates);
            }
        }

        private IEnumerable<Order> FiltrationByDateRange(int startNumber, int count, out int comonEntityCount, DateModel dates)
        {
            var from = DateTime.ParseExact(dates.DateFrom, DatePattern, CultureInfo.InvariantCulture);
            var to = DateTime.ParseExact(dates.DateTo, DatePattern, CultureInfo.InvariantCulture);
            comonEntityCount = _dbUoW.Orders.GetCountByCondition(s => s.Date >= from && s.Date <= to);
            return _dbUoW.Orders.GetRangeByConditionWithOrder(startNumber, count, s => s.Date >= from && s.Date <= to, d => d.Date);
        }

        private IEnumerable<Order> FiltrationByDateMoreThen(int startNumber, int count, out int comonEntityCount, DateModel dates)
        {
            var from = DateTime.ParseExact(dates.DateFrom, DatePattern, CultureInfo.InvariantCulture);
            comonEntityCount = _dbUoW.Orders.GetCountByCondition(s => s.Date >= from);
            return _dbUoW.Orders.GetRangeByConditionWithOrder(startNumber, count, s => s.Date >= from, d => d.Date);
        }

        private IEnumerable<Order> FiltrationByDateLessThen(int startNumber, int count, out int comonEntityCount, DateModel dates)
        {
            var to = DateTime.ParseExact(dates.DateTo, DatePattern, CultureInfo.InvariantCulture);
            comonEntityCount = _dbUoW.Orders.GetCountByCondition(s => s.Date <= to);
            return _dbUoW.Orders.GetRangeByConditionWithOrder(startNumber, count, s => s.Date <= to, d => d.Date);
        }
    }
}
