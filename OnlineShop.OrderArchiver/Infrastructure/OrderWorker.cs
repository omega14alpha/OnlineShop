using OnlineShop.DataAccess;
using OnlineShop.DataAccess.Entities;
using OnlineShop.OrderArchiver.Interfaces;
using System;
using System.Globalization;

namespace OnlineShop.OrderArchiver.Infrastructure
{
    public class OrderWorker : IOrderWorker
    {
        private const int ManagerNameIndex = 0;

        private const int DateIndex = 1;

        private const int ClientNameIndex = 2;

        private const int ItemNameIndex = 3;

        private const int ItemPriceIndex = 4;

        private const string dataFormat = "dd.MM.yyyy";

        private readonly DataBaseUoW _uow;

        private readonly object _locker;

        public OrderWorker(DataBaseUoW uow, object locker)
        {
            _uow = uow;
            _locker = locker;
        }

        public void SaveOrder(Guid sessionGuid, string data)
        {
            var dataArr = data.Split(';');
            lock (_locker)
            {
                var order = new Order()
                {
                    Manager = _uow.Managers.GetOrAddEntity(new Manager() { Surname = dataArr[ManagerNameIndex] }, s => s.Surname == dataArr[ManagerNameIndex]),
                    Date = DateTime.ParseExact(dataArr[DateIndex], dataFormat, CultureInfo.InvariantCulture),
                    Client = _uow.Clients.GetOrAddEntity(new Client() { Name = dataArr[ClientNameIndex] }, s => s.Name == dataArr[ClientNameIndex]),
                    Item = _uow.Items.GetOrAddEntity(new Item() { Name = dataArr[ItemNameIndex] }, s => s.Name == dataArr[ItemNameIndex]),
                    AmountOfMoney = Convert.ToDouble(dataArr[ItemPriceIndex], CultureInfo.GetCultureInfo("en-US")),
                    SessionId = sessionGuid
                };

                _uow.Orders.AddEntity(order);
                _uow.Save();
            }
        }

        public void DataRollback(Guid sessionGuid)
        {
            lock (_locker)
            {
                _uow.Orders.RemoveRange(sessionGuid);
                _uow.Save();
            }
        }
    }
}
