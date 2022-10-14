using System.Collections.Generic;

namespace TellDontAskKata.Main.Domain
{
    public class Order
    {
        public decimal Total { get; private set; }
        public string Currency { get; }
        public IList<OrderItem> Items { get; }
        public decimal Tax { get; private set; }
        public OrderStatus Status { get; set; }
        public int Id { get; }

        public Order(int id)
        {
            Id = id;
            Status = OrderStatus.Created;
            Items = new List<OrderItem>();
            Currency = "EUR";
            Total = 0m;
            Tax = 0m;
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
            Total += item.TaxedAmount;
            Tax += item.Tax;
        }
    }
}
