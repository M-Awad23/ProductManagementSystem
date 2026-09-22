using Microsoft.AspNetCore.Http;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IProductImageService
    {
        IEnumerable<ProductImage> GetAllProductImages();

        ProductImage? GetProductImageById(
            int id,
            string userId);

        Task<(bool Success, string? ErrorMessage)> AddProductImagesAsync(
            int productId,
            string userId,
            IEnumerable<IFormFile>? images);

        Task<(bool Success, string? ErrorMessage)> ReplaceProductImageAsync(
            int imageId,
            string userId,
            IFormFile? image);

        Task<bool> DeleteProductImageAsync(
            int imageId,
            string userId);

        Task<bool> SetPrimaryImageAsync(
            int imageId,
            string userId);
    }
}