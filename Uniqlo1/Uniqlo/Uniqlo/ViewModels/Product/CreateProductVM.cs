using System.ComponentModel.DataAnnotations;

namespace Uniqlo.ViewModels.Product
{
    public class CreateProductVM
    {
        [MaxLength(32, ErrorMessage = "Title length must be less than 32"), Required(ErrorMessage = "Basliq yazmaq vacibdir")]
        public string ProductName { get; set; }
        [MaxLength(400), Required]
        public string ProductDescription { get; set; }

        public int CostPrice { get; set; }

        public int SellPrice { get; set; }

        public int Quantity { get; set; }

        public int Discount { get; set; }

        public IFormFile CoverImage { get; set; } 

        public int CategoryID { get; set; }        
        
    }
}
