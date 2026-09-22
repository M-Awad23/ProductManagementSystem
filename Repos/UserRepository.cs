using Microsoft.AspNetCore.Identity;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User?> GetByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<string> GenerateChangeEmailTokenAsync(
            User user,
            string newEmail)
        {
            return await _userManager
                .GenerateChangeEmailTokenAsync(
                    user,
                    newEmail);
        }

        public async Task<IdentityResult> ChangeEmailAsync(
            User user,
            string newEmail,
            string token)
        {
            return await _userManager.ChangeEmailAsync(
                user,
                newEmail,
                token);
        }

        public async Task<IdentityResult> SetUserNameAsync(
            User user,
            string userName)
        {
            return await _userManager.SetUserNameAsync(
                user,
                userName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(
            User user,
            string currentPassword,
            string newPassword)
        {
            return await _userManager.ChangePasswordAsync(
                user,
                currentPassword,
                newPassword);
        }
    }
}