using ProductManagementSystem.Models;

namespace ProductManagementSystem.Services
{
    public interface IPdfService
    {
        byte[] GenerateProductPdf(Product product);
        byte[] GenerateProductsPdf(IEnumerable<Product> products);
    }
}