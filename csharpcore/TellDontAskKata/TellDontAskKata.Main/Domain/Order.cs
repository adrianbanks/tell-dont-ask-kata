using System.Collections.Generic;
using TellDontAskKata.Main.Service;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain
{
    public class Order
    {
        private readonly List<OrderItem> _items = new();

        public decimal Total { get; private set; }
        public string Currency { get; }
        public decimal Tax { get; private set; }
        public OrderStatus Status { get; private set; }
        public int Id { get; }
        public IEnumerable<OrderItem> Items => _items;

        public Order(int id)
        {
            Id = id;
            Status = OrderStatus.Created;
            Currency = "EUR";
            Total = 0m;
            Tax = 0m;
        }

        public void AddItem(OrderItem item)
        {
            _items.Add(item);
            Total += item.TaxedAmount;
            Tax += item.TaxAmount;
        }

        public void Approve(OrderApprovalRequest request)
        {
            if (Status == OrderStatus.Shipped)
            {
                throw new ShippedOrdersCannotBeChangedException();
            }

            if (request.Approved && Status == OrderStatus.Rejected)
            {
                throw new RejectedOrderCannotBeApprovedException();
            }

            if (!request.Approved && Status == OrderStatus.Approved)
            {
                throw new ApprovedOrderCannotBeRejectedException();
            }

            Status = request.Approved ? OrderStatus.Approved : OrderStatus.Rejected;
        }

        public void Ship(IShipmentService shipmentService)
        {
            if (Status == OrderStatus.Created || Status == OrderStatus.Rejected)
            {
                throw new OrderCannotBeShippedException();
            }

            if (Status == OrderStatus.Shipped)
            {
                throw new OrderCannotBeShippedTwiceException();
            }

            shipmentService.Ship(this);

            Status = OrderStatus.Shipped;
        }
    }
}
