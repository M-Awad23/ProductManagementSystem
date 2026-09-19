namespace ProductManagementSystem.Models
{
    public class Tag
    {
        public int Id { get; set; }


        public ICollection<ProductTag> ProductTags { get; set; }
            = new List<ProductTag>();
    }
}
