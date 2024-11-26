namespace Uniqlo.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = null!;
        public IEnumerable<Product>? Products { get; set; }
    }
}
