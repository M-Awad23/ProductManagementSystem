namespace ProductManagementSystem.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}