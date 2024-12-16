namespace Uniqlo.Models
{
    public class Basket:BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
        public decimal Subtotal { get; set; }
    }
}
