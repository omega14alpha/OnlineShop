using System.Collections.Generic;

namespace OnlineShop.OnlineClient.Pagination.Models
{
    public class PaginationModel<T>
    {
        public IEnumerable<T> Items { get; set; }

        public PageInfoModel PageInfo { get; set; }
    }
}
