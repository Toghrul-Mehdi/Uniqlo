using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Uniqlo.DataAccess;
using Uniqlo.ViewModel;
using Uniqlo.ViewModel.Basket;
using Uniqlo.ViewModels.Common;


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
                    Subtitle = x.Subtitle,
                    Title = x.Title,
                }).ToListAsync();
            vm.Products = await _context.Products
                .Where(x => !x.IsDeleted)
                .Select(x => new ProductItemVM
                {
                    Id = x.Id,
                    ImageUrl = x.CoverImage,
                    Name = x.ProductName,
                    Price = x.SellPrice,
                    Discount = x.Discount,
                    IsInStock = x.Quantity > 0,
                    CategoryID = x.CategoryID
                }).ToListAsync();
            vm.Categories = await _context.Categories
                .Where(x => !x.IsDeleted)
                .Select(x => new CategoryItemVM
                {
                    Id = x.Id,
                    CategoryName = x.CategoryName,
                }).ToListAsync();
            return View(vm);
        }
        public async Task<IActionResult> AddBasket(int id)
        {
            var basketItems = JsonSerializer.Deserialize<List<BasketProductItemVM>>(Request.Cookies["basket"] ?? "[]");

            var item = basketItems.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                item = new BasketProductItemVM(id);
                basketItems.Add(item);
            }
            item.Count++;
            Response.Cookies.Append("basket", JsonSerializer.Serialize(basketItems));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteBasket(int id)
        {
            var basketItems = JsonSerializer.Deserialize<List<BasketProductItemVM>>(Request.Cookies["basket"] ?? "[]");

            var item = basketItems!.FirstOrDefault(x => x.Id == id);
            if (item!.Count > 1)
            {
                item.Count--;
            }
            else
            {
                basketItems!.Remove(item);
            }

            Response.Cookies.Append("basket", JsonSerializer.Serialize(basketItems));

            return RedirectToAction(nameof(Index));
        }
    }
}
