using BeautyEcommerce.Data;
using Microsoft.AspNetCore.Mvc;
using BeautyEcommerce.Helpers;
using BeautyEcommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BeautyEcommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly Hshop2023Context _context;
        public CartController(Hshop2023Context context)
        {
            this._context = context;
        }
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>
            (MySetting.CART_KEY) ?? new List<CartItem>();    
        public IActionResult Index()
        {
            return View(Cart);
        }
        public IActionResult AddToCart(int id, int quantity=1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item == null)
            {
                var hangHoa = _context.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    MaHh = hangHoa.MaHh,
                    TenHh = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? string.Empty,
                    SoLuong = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            return RedirectToAction("Index");
        }
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout() { 
            if (Cart.Count == 0)
            {
                return RedirectToAction("/");
            }

            return View(Cart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model)
        {
            var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMER_ID).Value;
            var khachHang = new KhachHang();
            if (model.GiongKhachHang)
            {
                khachHang = _context.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
            }

            var hoadon = new HoaDon
            {
                MaKh = customerId,
                HoTen = model.HoTen ?? khachHang.HoTen,
                DiaChi = model.DiaChi ?? khachHang.DiaChi,
                DienThoai = model.DienThoai ?? khachHang.DienThoai,
                NgayDat = DateTime.Now,
                CachThanhToan = "COD",
                CachVanChuyen = "GRAB",
                MaTrangThai = 0,
                GhiChu = model.GhiChu
            };

            _context.Database.BeginTransaction();
            try
            {
                _context.Database.CommitTransaction();
                _context.Add(hoadon);
                _context.SaveChanges();
                var cthds = new List<ChiTietHd>();
                foreach (var item in Cart)
                {
                    cthds.Add(new ChiTietHd
                    {
                        MaHd = hoadon.MaHd,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia,
                        MaHh = item.MaHh,
                        GiamGia = 0
                    });
                }
                _context.AddRange(cthds);
                _context.SaveChanges();

                HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

                return View("Success");
            }
            catch
            {
                _context.Database.RollbackTransaction();
            }

            return View(Cart);
        }
    }
}
