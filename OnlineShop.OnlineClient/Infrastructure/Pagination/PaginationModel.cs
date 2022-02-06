using System.Collections.Generic;

namespace OnlineShop.OnlineClient.Infrastructure.Pagination
{
    public class PaginationModel<T>
    {
        public IEnumerable<T> Items { get; set; }

        public PageInfoModel PageInfo { get; set; }
    }
}
