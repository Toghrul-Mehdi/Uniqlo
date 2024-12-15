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
    public class SliderController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
            if (vm.File != null)
            {
                if (!vm.File.ContentType.StartsWith("image"))
                    ModelState.AddModelError("File", "File type must be image");
                if (vm.File.Length > 2 * 1024 * 1024)
                    ModelState.AddModelError("File", "File length must be less than 2Mb");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            string newFileName = Path.GetRandomFileName() + Path.GetExtension(vm.File.FileName);

            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "img", "sliders", newFileName)))
            {
                await vm.File.CopyToAsync(stream);
            }

            Slider slider = new Slider
            {
                ImageUrl = newFileName,
                Title = vm.Title,
                Subtitle = vm.Subtitle,
            };
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var data = await _context.Sliders.FindAsync(id);
            if (data is null) return View();
            _context.Sliders.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var data = await _context.Sliders
                .Where(c => c.Id == id.Value)
                .Select(x => new SliderUpdateVM
                {
                    Title = x.Title,
                    Subtitle = x.Subtitle,
                    ImageUrl = x.ImageUrl,
                })
                .FirstOrDefaultAsync();

            if (data is null) return NotFound();
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM vm)
        {
            if (!id.HasValue) return BadRequest();
            if (vm.File != null)
            {
                if (!vm.File.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("File", "File type must be an image");
                }
                if (vm.File.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("File", "File size must be less than 5mb");
                }
            }
            if (!ModelState.IsValid) return View(vm);
            var sliders = await _context.Sliders
                .Where(c => c.Id == id.Value)
                .FirstOrDefaultAsync();
            if (sliders is null) return NotFound();
            if (vm.File != null)
            {
                string path = Path.Combine(_env.WebRootPath, "img", "sliders", sliders.ImageUrl);
                using (Stream sr = System.IO.File.Create(path))
                {
                    await vm.File!.CopyToAsync(sr);
                }
            }
            sliders.Title = vm.Title;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int? id, SliderCreateVM vm)
        {
            var data = await _context.Sliders.FindAsync(id);
            if (data is null) return View();
            data.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Hide(int? id, SliderCreateVM vm)
        {
            var data = await _context.Sliders.FindAsync(id);
            if (data is null) return View();
            data.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
