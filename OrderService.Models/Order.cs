namespace OrderService.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public List<OrderItem> Items { get; set; } = new();
        public string PaymentMethod { get; set; } = "Card";
    }
}