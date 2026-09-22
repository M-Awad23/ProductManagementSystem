namespace ProductManagementSystem.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string UserId { get; set; }
<<<<<<< HEAD

        public ICollection<Product> Products { get; set; }
           = new List<Product>();

=======
        public User User { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
>>>>>>> f14cd75c120a9267dfbfa1b24486d7ca2f8f2d38
    }
}