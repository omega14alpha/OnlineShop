using System;

namespace OnlineShop.OnlineClient.Infrastructure.Pagination
{
    public class PageInfoModel
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages
        {
            get => (int)Math.Ceiling((double)TotalItems / PageSize);
        }
    }
}
