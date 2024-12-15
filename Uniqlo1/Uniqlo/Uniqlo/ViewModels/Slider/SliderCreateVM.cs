using System.ComponentModel.DataAnnotations;

namespace Uniqlo.ViewModel
{
    public class SliderCreateVM
    {
        [MaxLength(32, ErrorMessage = "Title length must be less than 32"), Required(ErrorMessage = "Basliq yazmaq vacibdir")]
        public string Title { get; set; }
        [MaxLength(64), Required]
        public string Subtitle { get; set; } = null!;
        [Required]
        public IFormFile File { get; set; }
    }
}
