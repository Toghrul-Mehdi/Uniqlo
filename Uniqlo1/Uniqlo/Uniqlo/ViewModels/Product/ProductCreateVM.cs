using System.ComponentModel.DataAnnotations;

namespace Uniqlo.ViewModel
{
    public class ProductCreateVM
    {
        [MaxLength(32,ErrorMessage ="Product Name's must be less than 32"),Required(ErrorMessage ="Product Name bosh ola bilmez!")]
        public string ProductName { get; set; }
        [MaxLength(400, ErrorMessage = "Product Description's must be less than 400"), Required(ErrorMessage = "Product Description bosh ola bilmez!")]
        public string ProductDescription { get; set; }
        [Required]
        public decimal CostPrice { get; set; }
        [Required]
        public decimal SellPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Discount { get; set; }
        [Required]
        public IFormFile? CoverImage { get; set; }

        public ICollection<IFormFile>? OtherFiles { get; set; }
        [Required]
        public int CategoryID { get; set; }
    }
}
