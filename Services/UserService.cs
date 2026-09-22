using Microsoft.AspNetCore.Identity;
using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByIdAsync(
            string userId)
        {
            return await _userRepository
                .GetByIdAsync(userId);
        }

        public async Task<User?> GetUserByEmailAsync(
            string email)
        {
            return await _userRepository
                .GetByEmailAsync(email);
        }

        public async Task<IdentityResult> UpdateUserAsync(
            User user)
        {
            return await _userRepository
                .UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangeEmailAsync(
            User user,
            string newEmail)
        {
            var existingUser =
                await _userRepository
                    .GetByEmailAsync(newEmail);

            if (existingUser != null &&
                existingUser.Id != user.Id)
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Description =
                            "That email is already being used."
                    });
            }

            var token =
                await _userRepository
                    .GenerateChangeEmailTokenAsync(
                        user,
                        newEmail);

            var emailResult =
                await _userRepository
                    .ChangeEmailAsync(
                        user,
                        newEmail,
                        token);

            if (!emailResult.Succeeded)
            {
                return emailResult;
            }

            var userNameResult =
                await _userRepository
                    .SetUserNameAsync(
                        user,
                        newEmail);

            return userNameResult;
        }

        public async Task<IdentityResult> ChangePasswordAsync(
            User user,
            string currentPassword,
            string newPassword)
        {
            return await _userRepository
                .ChangePasswordAsync(
                    user,
                    currentPassword,
                    newPassword);
        }
    }
}