using OnlineShop.BusinessLogic.Models.Chart;
using OnlineShop.DataAccess;
using System.Collections.Generic;

namespace OnlineShop.BusinessLogic
{
    public class ChartWorker
    {
        private readonly DataBaseUoW _dbUoW;

        public ChartWorker(DataBaseUoW dataBaseUoW)
        {
            _dbUoW = dataBaseUoW;
        }

        public IEnumerable<ChartDataUnitModel> GetSalesManagersSums()
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

            return data;
        }
    }
}
