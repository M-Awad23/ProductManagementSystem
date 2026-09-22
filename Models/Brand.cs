namespace ProductManagementSystem.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}