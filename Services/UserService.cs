using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class UserService : IUserService
    {
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxProfilePhotoSize = 5 * 1024 * 1024;

        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IWebHostEnvironment environment, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _environment = environment;
            _logger = logger;
        }

        public async Task<User?> GetUserByIdAsync(string userId) => await _userRepository.GetByIdAsync(userId);
        public async Task<User?> GetUserByEmailAsync(string email) => await _userRepository.GetByEmailAsync(email);
        public async Task<IdentityResult> UpdateUserAsync(User user) => await _userRepository.UpdateAsync(user);

        public async Task<IdentityResult> ChangeEmailAsync(User user, string newEmail)
        {
            var existingUser = await _userRepository.GetByEmailAsync(newEmail);

            if (existingUser != null && existingUser.Id != user.Id)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "That email is already being used."
                });
            }

            var token = await _userRepository.GenerateChangeEmailTokenAsync(user, newEmail);
            var emailResult = await _userRepository.ChangeEmailAsync(user, newEmail, token);

            if (!emailResult.Succeeded)
                return emailResult;

            return await _userRepository.SetUserNameAsync(user, newEmail);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword) =>
            await _userRepository.ChangePasswordAsync(user, currentPassword, newPassword);

        public async Task<(bool Success, string? ErrorMessage)> UploadProfilePhotoAsync(string userId, IFormFile? photo)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return (false, "User not found.");

            if (photo == null || photo.Length == 0)
                return (false, "Please select an image.");

            var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();

            if (!AllowedImageExtensions.Contains(extension))
                return (false, "Only JPG, JPEG, PNG, and GIF images are allowed.");

            if (photo.Length > MaxProfilePhotoSize)
                return (false, "Profile photo must be 5 MB or smaller.");

            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
            Directory.CreateDirectory(uploadPath);

            var oldPhotoUrl = user.ProfilePhotoUrl;
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadPath, fileName);

            try
            {
                await using var stream = new FileStream(filePath, FileMode.Create);
                await photo.CopyToAsync(stream);

                user.ProfilePhotoUrl = $"/uploads/profiles/{fileName}";
                var result = await _userRepository.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    return (false, string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                DeletePhysicalFile(oldPhotoUrl);

                _logger.LogInformation("Profile photo uploaded/replaced for UserId {UserId}.", userId);
                return (true, null);
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                _logger.LogError(ex, "Failed to upload profile photo for UserId {UserId}.", userId);
                return (false, "The profile photo could not be uploaded.");
            }
        }

        public async Task<bool> DeleteProfilePhotoAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return false;

            var oldPhotoUrl = user.ProfilePhotoUrl;
            user.ProfilePhotoUrl = null;

            var result = await _userRepository.UpdateAsync(user);

            if (!result.Succeeded)
                return false;

            DeletePhysicalFile(oldPhotoUrl);

            _logger.LogInformation("Profile photo deleted for UserId {UserId}.", userId);
            return true;
        }

        private void DeletePhysicalFile(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return;

            var relativePath = imageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
            var filePath = Path.Combine(_environment.WebRootPath, relativePath);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}