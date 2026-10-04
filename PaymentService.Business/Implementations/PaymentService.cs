using PaymentService.Business.Interfaces;
using PaymentService.Data.Repositories.Interfaces;
using PaymentService.Models;

namespace PaymentService.Business.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllAsync();
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _paymentRepository.GetByIdAsync(id);
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            payment.PaymentDate = DateTime.UtcNow;
            payment.PaymentStatus = "Paid";

            return await _paymentRepository.AddAsync(payment);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task DeletePaymentAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
            {
                throw new KeyNotFoundException(
                    $"Payment with ID {id} was not found.");
            }

            await _paymentRepository.DeleteAsync(payment);
        }
    }
}