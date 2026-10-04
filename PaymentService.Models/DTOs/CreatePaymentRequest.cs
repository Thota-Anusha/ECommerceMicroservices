namespace PaymentService.Models.DTOs
{
    public class CreatePaymentRequest
    {
        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = "Card";
    }
}