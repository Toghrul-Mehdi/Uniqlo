using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo.DataAccess;
using Uniqlo.ViewModels.Common;
using Uniqlo.ViewModels.Product;
using Uniqlo.ViewModels.Slider;

namespace Uniqlo.Controllers
{
    public class HomeController(UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new HomeVM();
            vm.Sliders = await _context.Sliders
                .Where(x => !x.IsDeleted)
                .Select(x => new SliderItemVM
                {
                    ImageUrl = x.ImageUrl,
                    Link = x.Link,
                    Subtitle = x.Description,
                    Title = x.Title,
                }).ToListAsync();
            vm.Products = await _context.Products
                .Where(x => !x.IsDeleted)
                .Select(x => new ProductItemVM
                {
                    Id = x.Id,
                    Name = x.ProductName,
                    Price = x.SellPrice,
                    Discount = x.Discount,
                    ImageUrl = x.CoverImage,
                    IsInStock = x.Quantity > 0
                }).ToListAsync();
            return View(vm);
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
