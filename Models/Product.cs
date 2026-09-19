using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }

        public User? User { get; set; }

        public int? CategoryId { get; set; }

        public Category? Category { get; set; }

        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

    

        public ICollection<ProductTag> ProductTags { get; set; }
            = new List<ProductTag>();
    }
}
