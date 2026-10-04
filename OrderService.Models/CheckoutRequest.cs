namespace OrderService.Models
{
    public class CheckoutRequest
    {
        public List<CheckoutItem> Items { get; set; } = new();
        public string PaymentMethod { get; set; } = "Card";
    }

    public class CheckoutItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}