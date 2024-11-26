using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo.DataAccess;
using Uniqlo.Models;
using Uniqlo.ViewModels.Product;

namespace Uniqlo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
    {        
        public async Task<IActionResult> Products()
        {
            return View(await _context.Products.Include(x=>x.Category).ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories =  await _context.Categories.Where(x=>!x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM vm)
        {
            if (!vm.CoverImage.ContentType.StartsWith("image"))
                ModelState.AddModelError("File", "File type must be image");
            if (vm.CoverImage.Length > 2 * 1024 * 1024)
                ModelState.AddModelError("File", "File length must be less than 2Mb");
            if (!ModelState.IsValid) return View();

            string newFileName = Path.GetRandomFileName() + Path.GetExtension(vm.CoverImage.FileName);

            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "img", "sliders", newFileName)))
            {
                await vm.CoverImage.CopyToAsync(stream);
            }

            Product product = new Product
            {
                CoverImage = newFileName,
                ProductName = vm.ProductName,
                ProductDescription = vm.ProductDescription,
                CostPrice = vm.CostPrice,
                SellPrice = vm.SellPrice,
                Discount =  vm.Discount,
                Quantity = vm.Quantity,
                CategoryID = vm.CategoryID,
            };  
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Products));
        }
    }
}
