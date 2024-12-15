namespace Uniqlo.Models
{
    public class Product : BaseEntity   
    {
        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; } = null!;
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public string CoverImage { get; set; } = null!;
        public int CategoryID { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<ProductImages>? Images { get; set; }
        public ICollection<ProductRatings>? ProductRatings { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
