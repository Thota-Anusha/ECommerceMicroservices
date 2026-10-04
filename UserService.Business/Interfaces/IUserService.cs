using UserService.Models.DTOs;

namespace UserService.Business.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> RegisterAsync(RegisterRequest request);

        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}