using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo.DataAccess;
using Uniqlo.Models;
using Uniqlo.ViewModels.Product;
using Uniqlo.ViewModels.Slider;

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

        public async Task<IActionResult> Update()
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View();            
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, CreateProductVM vm)
        {
            if (!ModelState.IsValid) return View();


            if (!vm.CoverImage.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("File", "File type must be image");
                return View();
            }

            if (vm.CoverImage.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("File", "File size must be less than 2MB");
                return View();
            }

            var data = await _context.Products.FindAsync(id);

            if (data is null) return View();

            string newFileName = Path.GetRandomFileName() + Path.GetExtension(vm.CoverImage.FileName);

            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "img", "sliders", newFileName)))
            {
                await vm.CoverImage.CopyToAsync(stream);
            }

            data.CoverImage = newFileName;
            data.ProductName = vm.ProductName;
            data.ProductDescription = vm.ProductDescription;
            data.CostPrice = vm.CostPrice;
            data.SellPrice = vm.SellPrice;
            data.Discount = vm.Discount;
            data.Quantity = vm.Quantity;
            data.CategoryID = vm.CategoryID;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Products));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var data = await _context.Products.FindAsync(id);

            if (data is null) return View();


            _context.Products.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Products));
        }

        public async Task<IActionResult> Hide(int id, CreateProductVM vm)
        {
            var data = await _context.Products.FindAsync(id);

            if (data is null) return View();

            data.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Products));
        }
        public async Task<IActionResult> Show(int id, CreateProductVM vm)
        {
            var data = await _context.Products.FindAsync(id);

            if (data is null) return View();

            data.IsDeleted = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Products));
        }
    }
}
