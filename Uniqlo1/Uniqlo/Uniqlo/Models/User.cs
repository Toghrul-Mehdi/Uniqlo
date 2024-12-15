using Microsoft.AspNetCore.Identity;

namespace Uniqlo.Models
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
        public string ProfileImageUrl { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<ProductRatings>? ProductRatings { get; set; }
    }
}
