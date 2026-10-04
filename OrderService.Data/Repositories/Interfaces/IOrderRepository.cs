using OrderService.Models;

namespace OrderService.Data.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(int id);

        Task<Order> AddAsync(Order order);

        Task UpdateAsync(Order order);

        Task DeleteAsync(Order order);
    }
}