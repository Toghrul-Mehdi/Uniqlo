namespace Uniqlo.Models
{
    public class Comment : BaseEntity
    {
        public string Comments { get; set; }
        public int Like { get; set; } = 0;
        public string UserId { get; set; } = null!;
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
