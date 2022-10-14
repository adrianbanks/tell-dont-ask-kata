namespace TellDontAskKata.Main.Domain
{
    public sealed class Product
    {
        public string Name { get; }
        public decimal Price { get; }
        public Category Category { get; }

        public Product(string name, decimal price, Category category)
        {
            Name = name;
            Price = price;
            Category = category;
        }
    }
}
