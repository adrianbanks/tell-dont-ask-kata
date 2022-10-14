namespace TellDontAskKata.Main.Domain
{
    public record OrderItem(Product Product, int Quantity)
    {
        public decimal TaxedAmount => Round(Product.UnitaryTaxedAmount * Quantity);

        public decimal TaxAmount => Round(Product.UnitaryTax * Quantity);

        private static decimal Round(decimal amount) => decimal.Round(amount, 2, System.MidpointRounding.ToPositiveInfinity);
    }
}
