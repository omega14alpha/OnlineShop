using OnlineShop.DataAccess.Contexts;
using OnlineShop.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.DataAccess.Repositories
{
    public class ForChartRepository
    {
        private DbOrderContext _context;

        private const string dataFormat = "dd.MM.yyyy";

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

        public IEnumerable<ManagerMonthModel> GetMonthSaleManagersData(string surname)
        {
            var id = _context.Managers.FirstOrDefault(m => m.Surname == surname).Id;
            return _context
                .Orders
                .Where(m => m.ManagerId == id)
                .GroupBy(d => new { Month = d.Date.Month, Year = d.Date.Year })
                .OrderBy(s => s.Key.Month)
                .Select(m => new ManagerMonthModel()
                {
                    Month = m.Key.Month,
                    Year = m.Key.Year,
                    Sum = m.Sum(p => p.AmountOfMoney)
                });
        }
    }
}
