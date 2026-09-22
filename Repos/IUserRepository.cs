using Microsoft.AspNetCore.Identity;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(string userId);

        Task<User?> GetByEmailAsync(string email);

        Task<IdentityResult> UpdateAsync(User user);

        Task<string> GenerateChangeEmailTokenAsync(
            User user,
            string newEmail);

        Task<IdentityResult> ChangeEmailAsync(
            User user,
            string newEmail,
            string token);

        Task<IdentityResult> SetUserNameAsync(
            User user,
            string userName);

        Task<IdentityResult> ChangePasswordAsync(
            User user,
            string currentPassword,
            string newPassword);
    }
}