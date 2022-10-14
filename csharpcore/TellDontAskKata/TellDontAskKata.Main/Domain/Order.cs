using System.Collections.Generic;
using System.Linq;
using TellDontAskKata.Main.Service;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain
{
    public class Order
    {
        private readonly List<OrderItem> _items = new();

        public int Id { get; }
        public string Currency { get; }

        public OrderStatus Status { get; private set; }
        public decimal Total => Items.Sum(item => item.TaxedAmount);
        public decimal Tax => Items.Sum(item => item.TaxAmount);

        public IEnumerable<OrderItem> Items => _items;

        public Order(int id)
        {
            Id = id;
            Currency = "EUR";
            Status = OrderStatus.Created;
        }

        public void AddItem(OrderItem item) => _items.Add(item);

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
