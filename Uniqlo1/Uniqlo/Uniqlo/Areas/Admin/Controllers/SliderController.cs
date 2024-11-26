using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo.DataAccess;
using Uniqlo.Models;
using Uniqlo.ViewModels.Slider;

namespace Uniqlo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
    {

        public async Task<IActionResult> CreateSlide()
        {
            return View(await _context.Sliders.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVM vm)
        {
            if (!vm.File.ContentType.StartsWith("image"))
                ModelState.AddModelError("File", "File type must be image");
            if (vm.File.Length > 2 * 1024 * 1024)
                ModelState.AddModelError("File", "File length must be less than 2Mb");
            if (!ModelState.IsValid) return View();

            string newFileName = Path.GetRandomFileName() + Path.GetExtension(vm.File.FileName);

            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "img", "sliders", newFileName)))
            {
                await vm.File.CopyToAsync(stream);
            }

            Slider slider = new Slider
            {
                ImageUrl = newFileName,
                Title = vm.Title,
                Description = vm.Subtitle,
                Link = vm.Link
            };
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CreateSlide));
        } 
        public IActionResult Updater()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Updater(int id, CreateSliderVM vm)
        {
            if (!ModelState.IsValid) return View();


            if (!vm.File.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("File", "File type must be image");
                return View();
            }

            if (vm.File.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("File", "File size must be less than 2MB");
                return View();
            }

            var data = await _context.Sliders.FindAsync(id);

            if (data is null) return View();

            string newFileName = Path.GetRandomFileName() + Path.GetExtension(vm.File.FileName);

            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "img", "sliders", newFileName)))
            {
                await vm.File.CopyToAsync(stream);
            }

            data.ImageUrl = newFileName;
            data.Link = vm.Link;
            data.Description = vm.Subtitle;
            data.Title = vm.Title;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(CreateSlide));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var data = await _context.Sliders.FindAsync(id);

            if (data is null) return View();


            _context.Sliders.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(CreateSlide));
        }

        public async Task<IActionResult> Hide(int id, CreateSliderVM vm)
        {
            var data = await _context.Sliders.FindAsync(id);

            if (data is null) return View();

            data.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(CreateSlide));
        }
        public async Task<IActionResult> Show(int id, CreateSliderVM vm)
        {
            var data = await _context.Sliders.FindAsync(id);

            if (data is null) return View();

            data.IsDeleted = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(CreateSlide));
        }


    }
}
