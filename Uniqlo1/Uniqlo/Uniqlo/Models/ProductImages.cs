namespace Uniqlo.Models
{
    public class ProductImages : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ImageUrl { get; set; }
    }
}
