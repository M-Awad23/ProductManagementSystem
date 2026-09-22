using Microsoft.AspNetCore.Identity;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(string userId);

        Task<User?> GetUserByEmailAsync(string email);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangeEmailAsync(
            User user,
            string newEmail);

        Task<IdentityResult> ChangePasswordAsync(
            User user,
            string currentPassword,
            string newPassword);
    }
}