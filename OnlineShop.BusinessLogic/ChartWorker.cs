using OnlineShop.BusinessLogic.Models.Chart;
using OnlineShop.DataAccess;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OnlineShop.BusinessLogic
{
    public class ChartWorker
    {
        private readonly DataBaseUoW _dbUoW;

        public ChartWorker(DataBaseUoW dataBaseUoW)
        {
            _dbUoW = dataBaseUoW;
        }

        public ChartData GetSalesManagersSums()
        {
            var result = _dbUoW.ChartRepository.GetManagersData();
            var data = new List<ChartDataUnitModel>();
            foreach (var item in result)
            {
                data.Add(new ChartDataUnitModel()
                {
                    DimensionOne = item.Surname,
                    Quantity = item.Price
                });
            }

            return new ChartData() { ChartType = "bar", HeadLine = "Sales managers", Units = data };
        }

        public IEnumerable<string> GetManagerList() => 
            _dbUoW.Managers.GetEntities().Select(s => s.Surname);

        public ChartData GetMonthSalesManagers(string manager)
        {
            var result = _dbUoW.ChartRepository.GetMonthSaleManagersData(manager);
            var data = new List<ChartDataUnitModel>();
            foreach (var item in result)
            {
                data.Add(new ChartDataUnitModel()
                {
                    DimensionOne = CreateDate(item.Month, item.Year),
                    Quantity = item.Sum
                });
            }

            return new ChartData() { ChartType = "line", HeadLine = $"{manager} sales", Units = data };
        }

        private string CreateDate(int month, int year) =>
            $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year}";
    }
}
