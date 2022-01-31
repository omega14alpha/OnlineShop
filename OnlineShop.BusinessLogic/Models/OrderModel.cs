using System;

namespace OnlineShop.BusinessLogic.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string Date { get; set; }

        public string Manager { get; set; }

        public string Client { get; set; }

        public string Item { get; set; }

        public double AmountOfMoney { get; set; }
    }
}
