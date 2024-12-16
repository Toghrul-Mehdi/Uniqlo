using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo.ViewModel.Basket;
using Uniqlo.ViewModel;
using Uniqlo.ViewModels.Common;
using Uniqlo.DataAccess;
using System.Text.Json;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Uniqlo.Models;
using System.Security.Claims;

namespace Uniqlo.Controllers
{
    public class ShopController(UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new HomeVM();
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
            if (!User.Identity!.IsAuthenticated)
            {
                var basketItems = JsonSerializer.Deserialize<List<BasketProductItemVM>>(Request.Cookies["basket"] ?? "[]");

                var item = basketItems!.FirstOrDefault(x => x.Id == id);
                if (item == null)
                {
                    item = new BasketProductItemVM(id);
                    basketItems!.Add(item);
                }
                item.Count++;
                Response.Cookies.Append("basket", JsonSerializer.Serialize(basketItems));

                return RedirectToAction(nameof(Details), new { Id = id });
            }
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return BadRequest();
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                var BasketItem = await _context.Baskets
                    .FirstOrDefaultAsync(x=>x.UserId == userId && x.ProductId==id);
                if(BasketItem == null)
                {
                    var baskets = new Basket
                    {
                        UserId = userId,
                        ProductId = id,
                        Count = 1,
                        Subtotal = product.SellPrice
                    };
                    _context.Baskets.Add(baskets);
                }
                else
                {
                    BasketItem.Count++;
                    BasketItem.Subtotal += product.SellPrice;
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { Id = id });

            }
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
