namespace TellDontAskKata.Main.Domain
{
    public record OrderItem(Product Product, int Quantity, decimal TaxedAmount, decimal Tax);
}
