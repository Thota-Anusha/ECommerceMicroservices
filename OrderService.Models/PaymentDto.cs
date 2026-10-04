namespace OrderService.Models
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string PaymentStatus { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; }
    }
}