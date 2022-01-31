using Microsoft.EntityFrameworkCore;
using OnlineShop.DataAccess.Entities;
using OnlineShop.OrderArchiver.Interfaces;
using System;
using System.Globalization;
using System.Linq;

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


        private readonly DbContext _dbContext;

        private readonly object _locker;

        public OrderWorker(DbContext dbContext, object locker)
        {
            _dbContext = dbContext;
            _locker = locker;
        }

        public void SaveOrder(Guid sessionGuid, string data)
        {
            var dataArr = data.Split(';');
            lock (_locker)
            {
                var order = new Order()
                {
                    Manager = GetOrSaveModel(new Manager() { Surname = dataArr[ManagerNameIndex] }, s => s.Surname == dataArr[ManagerNameIndex]),
                    Date = DateTime.ParseExact(dataArr[DateIndex], dataFormat, CultureInfo.InvariantCulture),
                    Client = GetOrSaveModel(new Client() { Name = dataArr[ClientNameIndex] }, s => s.Name == dataArr[ClientNameIndex]),
                    Item = GetOrSaveModel(new Item() { Name = dataArr[ItemNameIndex] }, s => s.Name == dataArr[ItemNameIndex]),
                    AmountOfMoney = Convert.ToDouble(dataArr[ItemPriceIndex], CultureInfo.GetCultureInfo("en-US")),
                    SessionId = sessionGuid
                };

                _dbContext.Add(order);
                _dbContext.SaveChanges();
            }
        }

        private T GetOrSaveModel<T>(T entity, Func<T, bool> func) where T : class
        {
            return _dbContext.Set<T>().FirstOrDefault(func) ??
                _dbContext.Set<T>().Add(entity).Entity;
        }

        public void DataRollback(Guid sessionGuid)
        {
            lock (_locker)
            {
                var entity = _dbContext.Set<Order>().Where(s => s.SessionId == sessionGuid);
                _dbContext.RemoveRange(entity);
                _dbContext.SaveChanges();
            }
        }
    }
}
