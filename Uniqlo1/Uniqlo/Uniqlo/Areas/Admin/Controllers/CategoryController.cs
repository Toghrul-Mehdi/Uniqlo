using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo.DataAccess;
using Uniqlo.Models;
using Uniqlo.ViewModels.Category;
using Uniqlo.ViewModels.Product;

namespace Uniqlo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Categories()
        {
            return View(await _context.Categories.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM vm)
        {            
            if (!ModelState.IsValid) return View();          

            Category category = new Category
            {
                CategoryName = vm.CategoryName,
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Categories));
        }

        

        public async Task<IActionResult> Update(int id,CreateCategoryVM vm)
        {
            if (!ModelState.IsValid) return View();

            var data = await _context.Categories.FindAsync(id);

            if (data is null) return View();

            data.CategoryName = vm.CategoryName;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var data = await _context.Categories.FindAsync(id);

            if (data is null) return View();


            _context.Categories.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Categories));
        }
    }
}
