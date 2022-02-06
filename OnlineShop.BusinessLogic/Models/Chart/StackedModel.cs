using System.Collections.Generic;

namespace OnlineShop.BusinessLogic.Models.Chart
{
    public class StackedModel
    {
        public string StackedDimensionOne { get; set; }

        public List<ChartDataUnitModel> ChartData { get; set; }
    }
}
