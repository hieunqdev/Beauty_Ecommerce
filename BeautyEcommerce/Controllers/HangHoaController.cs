using BeautyEcommerce.Data;
using BeautyEcommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Detail(int id)
        {
            var data = _context.HangHoas
                .Include(product => product.MaLoaiNavigation) 
                .SingleOrDefault(product => product.MaHh == id);
            if (data == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }
            var result = new ChiTietHangHoaVM
            {
                MaHh = data.MaHh,
                TenHh = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? string.Empty,
                Hinh = data.Hinh ?? string.Empty,
                MoTaNgan = data.MoTa ?? string.Empty,
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10,
                DiemDanhGia = 5
            };
            return View(result);
        }
    }
}
