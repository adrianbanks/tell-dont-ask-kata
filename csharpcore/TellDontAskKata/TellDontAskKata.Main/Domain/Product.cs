namespace TellDontAskKata.Main.Domain
{
    public record Product(string Name, decimal Price, Category Category)
    {
        public decimal UnitaryTax => Round(Price / 100m * Category.TaxPercentage);

        public decimal UnitaryTaxedAmount => Round(Price + UnitaryTax);

        private static decimal Round(decimal amount) => decimal.Round(amount, 2, System.MidpointRounding.ToPositiveInfinity);
    }
}
