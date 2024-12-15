using System.ComponentModel.DataAnnotations;

namespace Uniqlo.ViewModel
{
    public class SliderUpdateVM
    {
        [MaxLength(32, ErrorMessage = "Title length must be less than 32"), Required(ErrorMessage = "Basliq yazmaq vacibdir")]
        public string Title { get; set; }
        [MaxLength(64), Required]
        public string Subtitle { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public IFormFile? File { get; set; } 
    }
}
