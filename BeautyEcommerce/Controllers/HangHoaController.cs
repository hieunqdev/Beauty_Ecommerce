using BeautyEcommerce.Data;
using BeautyEcommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BeautyEcommerce.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Hshop2023Context _context;
        public HangHoaController(Hshop2023Context context)
        {
            this._context = context;
        }
        public IActionResult Index(int? loai)
        {
            var products = _context.HangHoas.AsQueryable();
            if (loai.HasValue)
            {
                products = products.Where(productType => productType.MaLoai == loai.Value);
            }
            var result = products.Select(product => new HangHoaVM
            {
                MaHh = product.MaHh,
                TenHh = product.TenHh,
                Hinh = product.Hinh,
                DonGia = product.DonGia ?? 0,
                MoTaNgan = product.MoTa ?? "",
                TenLoai = product.MaLoaiNavigation.TenLoai
            });
            return View(result);
        }
        public IActionResult Search(string query)
        {
            var products = _context.HangHoas.AsQueryable();
            if (query != null)
            {
                products = products.Where(product => product.TenHh.Contains(query));
            }
            var result = products.Select(product => new HangHoaVM
            {
                MaHh = product.MaHh,
                TenHh = product.TenHh,
                Hinh = product.Hinh,
                DonGia = product.DonGia ?? 0,
                MoTaNgan = product.MoTa ?? "",
                TenLoai = product.MaLoaiNavigation.TenLoai
            });
            return View(result);
        }
    }
}
