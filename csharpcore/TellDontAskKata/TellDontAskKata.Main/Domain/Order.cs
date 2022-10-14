using System.Collections.Generic;
using System.Linq;
using TellDontAskKata.Main.Service;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain
{
    public class Order
    {
        public int Id { get; }
        public string Currency { get; }
        public OrderStatus Status { get; }

        public IEnumerable<OrderItem> Items { get; }

        public decimal Total => Items.Sum(item => item.TaxedAmount);
        public decimal Tax => Items.Sum(item => item.TaxAmount);

        private Order(int id, OrderStatus status, params OrderItem[] items)
        {
            Id = id;
            Currency = "EUR";
            Status = status;
            Items = items;
        }

        public Order(int id, params OrderItem[] items) : this(id, OrderStatus.Created, items)
        { }

        public Order Approve(OrderApprovalRequest request)
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

            var newStatus = request.Approved ? OrderStatus.Approved : OrderStatus.Rejected;
            return new Order(1, newStatus, Items.ToArray());
        }

        public Order Ship(IShipmentService shipmentService)
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

            return new Order(1, OrderStatus.Shipped, Items.ToArray());
        }
    }
}
