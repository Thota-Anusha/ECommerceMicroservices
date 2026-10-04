using UserService.Models;

namespace UserService.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int userId);

        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByUsernameAsync(string username);

        Task<User> AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(User user);
    }
}