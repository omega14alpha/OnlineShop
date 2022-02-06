using OnlineShop.DataAccess.Contexts;
using OnlineShop.DataAccess.Entities;
using OnlineShop.DataAccess.Interfaces;
using OnlineShop.DataAccess.Repositories;
using System;

namespace OnlineShop.DataAccess
{
    public class DataBaseUoW : IDisposable
    {
        private readonly DbOrderContext _dbContext;

        private IRepository<Order> _orderRepository;  

        private IRepository<Client> _clientRepository;  

        private IRepository<Manager> _managerRepository;  

        private IRepository<Item> _itemRepository;

        private ForChartRepository _chartRepository;

        private bool _disposed;

        public DataBaseUoW(DbOrderContext dbContext)
        {
            _dbContext = dbContext;

            ForChartRepository repository = new ForChartRepository(dbContext);
            repository.GetManagersData();
        }

        public IRepository<Order> Orders => _orderRepository ?? new DbRepository<Order>(_dbContext);

        public IRepository<Client> Clients => _clientRepository ?? new DbRepository<Client>(_dbContext);

        public IRepository<Manager> Managers => _managerRepository ?? new DbRepository<Manager>(_dbContext);

        public IRepository<Item> Items => _itemRepository ?? new DbRepository<Item>(_dbContext);

        public ForChartRepository ChartRepository => _chartRepository ?? new ForChartRepository(_dbContext);

        public void Save() => _dbContext.SaveChanges();

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_disposed)
                {
                    _dbContext.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
