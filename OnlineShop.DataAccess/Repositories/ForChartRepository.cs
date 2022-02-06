using OnlineShop.DataAccess.Contexts;
using OnlineShop.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.DataAccess.Repositories
{
    public class ForChartRepository
    {
        private DbOrderContext _context;

        public ForChartRepository(DbOrderContext context)
        {
            _context = context;
        }

        public IEnumerable<ManagerPricesModel> GetManagersData() =>
            _context
            .Orders
            .GroupBy(s => s.Manager.Surname)
            .Select(s => new ManagerPricesModel()
            {
                Surname = s.Key,
                Price = s.Sum(p => p.AmountOfMoney)
            });
        

    }
}
