using ProductManagementSystem.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ProductManagementSystem.Services
{
    public class PdfService : IPdfService
    {
        public byte[] GenerateProductPdf(Product product)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    page.Header().Text("PRODUCT DETAILS").FontSize(22).Bold();

                    page.Content().PaddingVertical(20).Column(column =>
                    {
                        column.Spacing(10);
                        column.Item().Text($"Product ID: {product.Id}");
                        column.Item().Text($"Name: {product.Name}");
                        column.Item().Text($"Description: {product.Description}");
                        column.Item().Text($"Price: ${product.Price:N2}");
                        column.Item().Text($"Quantity: {product.Quantity}");
                        column.Item().Text($"Created: {product.CreatedAt:yyyy-MM-dd HH:mm}");
                        column.Item().Text($"Category: {product.Category?.Name ?? "N/A"}");
                        column.Item().Text($"Brand: {product.Brand?.Name ?? "N/A"}");
                        column.Item().Text($"Supplier: {product.Supplier?.Name ?? "N/A"}");

                        var tagNames = product.ProductTags
                            .Select(pt => pt.Tag?.Name)
                            .Where(name => !string.IsNullOrWhiteSpace(name))
                            .ToList();

                        if (tagNames.Any())
                        {
                            column.Item().Text($"Tags: {string.Join(", ", tagNames)}");
                        }

                        foreach (var productImage in product.ProductImages)
                        {
                            var imagePath = Path.Combine(
                                Directory.GetCurrentDirectory(),
                                "wwwroot",
                                productImage.ImageUrl.TrimStart('/').Replace(
                                    "/",
                                    Path.DirectorySeparatorChar.ToString()));

                            if (File.Exists(imagePath))
                            {
                                column.Item().Width(180).Image(imagePath);
                            }
                        }
                    });

                    page.Footer().AlignCenter().Text("Product Management System");
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerateProductsPdf(IEnumerable<Product> products)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.Header().Text("PRODUCT REPORT").FontSize(22).Bold();

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        foreach (var product in products)
                        {
                            column.Item().BorderBottom(1).PaddingBottom(8).Column(productColumn =>
                            {
                                productColumn.Item().Text($"{product.Id} - {product.Name}").Bold();
                                productColumn.Item().Text($"Price: ${product.Price:N2} | Quantity: {product.Quantity}");
                                productColumn.Item().Text($"Category: {product.Category?.Name ?? "N/A"}");
                                productColumn.Item().Text($"Brand: {product.Brand?.Name ?? "N/A"}");
                                productColumn.Item().Text($"Supplier: {product.Supplier?.Name ?? "N/A"}");

                                var tagNames = product.ProductTags
                                    .Select(pt => pt.Tag?.Name)
                                    .Where(name => !string.IsNullOrWhiteSpace(name))
                                    .ToList();

                                if (tagNames.Any())
                                {
                                    productColumn.Item().Text($"Tags: {string.Join(", ", tagNames)}");
                                }

                                var firstImage = product.ProductImages.FirstOrDefault();
                                if (firstImage != null)
                                {
                                    var imagePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot",
                                        firstImage.ImageUrl.TrimStart('/').Replace(
                                            "/",
                                            Path.DirectorySeparatorChar.ToString()));

                                    if (File.Exists(imagePath))
                                    {
                                        productColumn.Item().Width(100).Image(imagePath);
                                    }
                                }
                            });
                        }
                    });

                    page.Footer().AlignCenter().Text("Product Management System");
                });
            });

            return document.GeneratePdf();
        }
    }
}