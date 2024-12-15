using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo.DataAccess;
using Uniqlo.Models;
using Uniqlo.ViewModel;


namespace Uniqlo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid) return View();

            Category category = new Category
            {
                CategoryName = vm.CategoryName
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var data = await _context.Categories
                .Where(x => x.Id == id.Value)
                .Select(y => new CategoryUpdateVM
                {
                    CategoryName = y.CategoryName
                }).FirstOrDefaultAsync();
            if (data is null) return NotFound();
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVM vm)
        {
            if (!id.HasValue) return BadRequest();
            if (!ModelState.IsValid) return View(vm);
            var categories = await _context.Categories
                .Where(c => c.Id == id.Value)
                .FirstOrDefaultAsync();
            if (categories is null) return NotFound();
            categories.CategoryName = vm.CategoryName;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var data = await _context.Categories.FindAsync(id);
            if (data is null) return NotFound();
            _context.Categories.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
