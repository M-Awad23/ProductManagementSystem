using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(string userId);
        Task<User?> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
    }
}