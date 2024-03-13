using Microsoft.AspNetCore.Mvc;

namespace BeautyEcommerce.Controllers
{
    public class HangHoaController : Controller
    {
        public IActionResult Index(int? loai)
        {
            return View();
        }
    }
}
