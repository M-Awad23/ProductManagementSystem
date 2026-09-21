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

                    page.Header()
                        .Text("PRODUCT DETAILS")
                        .FontSize(22)
                        .Bold();

                    page.Content()
                        .PaddingVertical(20)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            column.Item()
                                .Text($"Product ID: {product.Id}");

                            column.Item()
                                .Text($"Name: {product.Name}");

                            column.Item()
                                .Text($"Description: {product.Description}");

                            column.Item()
                                .Text($"Price: ${product.Price:N2}");

                            column.Item()
                                .Text($"Quantity: {product.Quantity}");

                            column.Item()
                                .Text(
                                    $"Created: {product.CreatedAt:yyyy-MM-dd HH:mm}"
                                );
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text("Product Management System");
                });
            });

            return document.GeneratePdf();
        }
    }
}