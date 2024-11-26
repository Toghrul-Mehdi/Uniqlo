using System.ComponentModel.DataAnnotations;

namespace Uniqlo.ViewModels.Category
{
    public class CreateCategoryVM
    {
        [MaxLength(32, ErrorMessage = "Title length must be less than 32"), Required(ErrorMessage = "Basliq yazmaq vacibdir")]
        public string CategoryName { get; set; } = null!;
    }
}
