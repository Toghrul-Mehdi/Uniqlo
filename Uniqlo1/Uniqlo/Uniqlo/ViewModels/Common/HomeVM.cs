using Uniqlo.ViewModels.Category;
using Uniqlo.ViewModels.Product;
using Uniqlo.ViewModels.Slider;

namespace Uniqlo.ViewModels.Common
{
    public class HomeVM
    {
        public IEnumerable<SliderItemVM> Sliders { get; set; }
        public IEnumerable<ProductItemVM> Products { get; set; }
        
    }
}
