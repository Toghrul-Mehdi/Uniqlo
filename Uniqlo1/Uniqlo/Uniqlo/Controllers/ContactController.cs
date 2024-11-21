using Microsoft.AspNetCore.Mvc;

namespace Uniqlo.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
