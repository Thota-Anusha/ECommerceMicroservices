namespace PaymentService.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = "Card";

        public string PaymentStatus { get; set; } = "Pending";

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}