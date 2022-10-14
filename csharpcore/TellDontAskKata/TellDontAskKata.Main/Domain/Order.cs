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
        public OrderStatus Status { get; private set; }

        public IEnumerable<OrderItem> Items { get; }

        public decimal Total => Items.Sum(item => item.TaxedAmount);
        public decimal Tax => Items.Sum(item => item.TaxAmount);

        public Order(int id, params OrderItem[] items)
        {
            Id = id;
            Currency = "EUR";
            Status = OrderStatus.Created;
            Items = items;
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
