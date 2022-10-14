namespace TellDontAskKata.Main.Domain
{
    public sealed class OrderItem
    {
        public Product Product { get; }
        public int Quantity { get; }
        public decimal TaxedAmount { get; }
        public decimal Tax { get; }

        public OrderItem(Product product, int quantity, decimal taxedAmount, decimal tax)
        {
            Product = product;
            Quantity = quantity;
            TaxedAmount = taxedAmount;
            Tax = tax;
        }
    }
}
