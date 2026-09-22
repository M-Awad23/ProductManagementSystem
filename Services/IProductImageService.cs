<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Http;
=======
using Microsoft.AspNetCore.Http;
>>>>>>> 93905db3c400d3eb96297067c57aa400de7f2b71
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IProductImageService
    {
        IEnumerable<ProductImage> GetAllProductImages();
<<<<<<< HEAD

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
=======
        ProductImage? GetProductImageById(int id, string userId);
        Task<(bool Success, string? ErrorMessage)> AddProductImagesAsync(int productId, string userId, IEnumerable<IFormFile>? images);
        Task<(bool Success, string? ErrorMessage)> ReplaceProductImageAsync(int imageId, string userId, IFormFile? image);
        Task<bool> DeleteProductImageAsync(int imageId, string userId);
        Task<bool> SetPrimaryImageAsync(int imageId, string userId);
>>>>>>> 93905db3c400d3eb96297067c57aa400de7f2b71
    }
}