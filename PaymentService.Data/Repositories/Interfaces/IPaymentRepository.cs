using PaymentService.Models;

namespace PaymentService.Data.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();

        Task<Payment?> GetByIdAsync(int id);

        Task<Payment> AddAsync(Payment payment);

        Task UpdateAsync(Payment payment);

        Task DeleteAsync(Payment payment);
    }
}