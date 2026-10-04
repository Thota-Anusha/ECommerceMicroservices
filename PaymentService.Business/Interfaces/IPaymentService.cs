using PaymentService.Models;

namespace PaymentService.Business.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();

        Task<Payment?> GetPaymentByIdAsync(int id);

        Task<Payment> CreatePaymentAsync(Payment payment);

        Task UpdatePaymentAsync(Payment payment);

        Task DeletePaymentAsync(int id);
    }
}