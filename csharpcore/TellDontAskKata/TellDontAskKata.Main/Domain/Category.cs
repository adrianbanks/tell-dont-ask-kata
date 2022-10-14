namespace TellDontAskKata.Main.Domain
{
    public sealed class Category
    {
        public string Name { get; }
        public decimal TaxPercentage { get; }

        public Category(string name, decimal taxPercentage)
        {
            Name = name;
            TaxPercentage = taxPercentage;
        }
    }
}
