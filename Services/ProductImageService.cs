using Microsoft.AspNetCore.Http;
using ProductManagementSystem.Models;
using ProductManagementSystem.Repos;

namespace ProductManagementSystem.Services
{
    public class ProductImageService : IProductImageService
    {
        private static readonly string[] AllowedImageExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif"
        };

        private const long MaxProductImageSize = 5 * 1024 * 1024;

        private readonly IProductImageRepository _productImageRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ProductImageService> _logger;

        public ProductImageService(
            IProductImageRepository productImageRepository,
            IWebHostEnvironment environment,
            ILogger<ProductImageService> logger)
        {
            _productImageRepository = productImageRepository;
            _environment = environment;
            _logger = logger;
        }

        public IEnumerable<ProductImage> GetAllProductImages()
        {
            return _productImageRepository.GetAllProductImages();
        }

        public ProductImage? GetProductImageById(
            int id,
            string userId)
        {
            return _productImageRepository.GetProductImageById(
                id,
                userId);
        }

        public async Task<(bool Success, string? ErrorMessage)> AddProductImagesAsync(
            int productId,
            string userId,
            IEnumerable<IFormFile>? images)
        {
            if (images == null)
            {
                return (true, null);
            }

            foreach (var image in images)
            {
                if (image == null || image.Length == 0)
                {
                    continue;
                }

                var validationError = ValidateImage(image);

                if (validationError != null)
                {
                    return (false, validationError);
                }
            }

            var uploadPath = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "products");

            Directory.CreateDirectory(uploadPath);

            foreach (var image in images)
            {
                if (image == null || image.Length == 0)
                {
                    continue;
                }

                var extension =
                    Path.GetExtension(image.FileName)
                        .ToLowerInvariant();

                var fileName =
                    $"{Guid.NewGuid()}{extension}";

                var filePath =
                    Path.Combine(uploadPath, fileName);

                try
                {
                    await using var stream =
                        new FileStream(
                            filePath,
                            FileMode.Create);

                    await image.CopyToAsync(stream);

                    var productImage = new ProductImage
                    {
                        ImageUrl =
                            $"/uploads/products/{fileName}",

                        ProductId = productId
                    };

                    _productImageRepository
                        .AddProductImage(productImage);

                    _logger.LogInformation(
                        "Product image uploaded for ProductId {ProductId} by UserId {UserId}.",
                        productId,
                        userId);
                }
                catch (Exception ex)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    _logger.LogError(
                        ex,
                        "Failed to upload product image for ProductId {ProductId} by UserId {UserId}.",
                        productId,
                        userId);

                    return (
                        false,
                        "The product image could not be uploaded."
                    );
                }
            }

            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> ReplaceProductImageAsync(
            int imageId,
            string userId,
            IFormFile? image)
        {
            var existingImage =
                _productImageRepository
                    .GetProductImageById(
                        imageId,
                        userId);

            if (existingImage == null)
            {
                return (false, "Image not found.");
            }

            if (image == null || image.Length == 0)
            {
                return (false, "Please select an image.");
            }

            var validationError =
                ValidateImage(image);

            if (validationError != null)
            {
                return (false, validationError);
            }

            var uploadPath = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "products");

            Directory.CreateDirectory(uploadPath);

            var extension =
                Path.GetExtension(image.FileName)
                    .ToLowerInvariant();

            var newFileName =
                $"{Guid.NewGuid()}{extension}";

            var newFilePath =
                Path.Combine(
                    uploadPath,
                    newFileName);

            try
            {
                await using (var stream =
                    new FileStream(
                        newFilePath,
                        FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                DeletePhysicalFile(
                    existingImage.ImageUrl);

                existingImage.ImageUrl =
                    $"/uploads/products/{newFileName}";

                _productImageRepository
                    .UpdateProductImage(existingImage);

                _logger.LogInformation(
                    "Product image {ImageId} replaced by UserId {UserId}.",
                    imageId,
                    userId);

                return (true, null);
            }
            catch (Exception ex)
            {
                if (File.Exists(newFilePath))
                {
                    File.Delete(newFilePath);
                }

                _logger.LogError(
                    ex,
                    "Failed to replace product image {ImageId} for UserId {UserId}.",
                    imageId,
                    userId);

                return (
                    false,
                    "The product image could not be replaced."
                );
            }
        }

        public Task<bool> DeleteProductImageAsync(
            int imageId,
            string userId)
        {
            var image =
                _productImageRepository
                    .GetProductImageById(
                        imageId,
                        userId);

            if (image == null)
            {
                return Task.FromResult(false);
            }

            DeletePhysicalFile(
                image.ImageUrl);

            _productImageRepository
                .DeleteProductImage(
                    imageId,
                    userId);

            _logger.LogInformation(
                "Product image {ImageId} deleted by UserId {UserId}.",
                imageId,
                userId);

            return Task.FromResult(true);
        }

        public Task<bool> SetPrimaryImageAsync(
            int imageId,
            string userId)
        {
            var image =
                _productImageRepository
                    .GetProductImageById(
                        imageId,
                        userId);

            if (image == null)
            {
                return Task.FromResult(false);
            }

            _productImageRepository
                .SetPrimaryImage(
                    imageId,
                    userId);

            _logger.LogInformation(
                "Product image {ImageId} set as primary by UserId {UserId}.",
                imageId,
                userId);

            return Task.FromResult(true);
        }

        private static string? ValidateImage(
            IFormFile image)
        {
            var extension =
                Path.GetExtension(image.FileName)
                    .ToLowerInvariant();

            if (!AllowedImageExtensions.Contains(extension))
            {
                return
                    "Only JPG, JPEG, PNG, and GIF images are allowed.";
            }

            if (image.Length > MaxProductImageSize)
            {
                return
                    "Product images must be 5 MB or smaller.";
            }

            return null;
        }

        private void DeletePhysicalFile(
            string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            var relativePath =
                imageUrl
                    .TrimStart('/')
                    .Replace(
                        "/",
                        Path.DirectorySeparatorChar
                            .ToString());

            var filePath =
                Path.Combine(
                    _environment.WebRootPath,
                    relativePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}