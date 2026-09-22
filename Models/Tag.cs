namespace ProductManagementSystem.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
<<<<<<< HEAD

        public ICollection<ProductTag> ProductTags { get; set; }
            = new List<ProductTag>();
=======
        public User User { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
>>>>>>> f14cd75c120a9267dfbfa1b24486d7ca2f8f2d38
    }
}