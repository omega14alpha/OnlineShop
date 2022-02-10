using System.Collections.Generic;

namespace OnlineShop.BusinessLogic.Models.Chart
{
    public class ChartData
    {
        public string ChartType { get; set; }

        public string HeadLine { get; set; }

        public List<ChartDataUnitModel> Units { get; set; }
    }
}
