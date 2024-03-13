using BeautyEcommerce.Data;
using BeautyEcommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BeautyEcommerce.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly Hshop2023Context _context;
        public MenuLoaiViewComponent(Hshop2023Context context)
        {
            this._context = context;
        }
        public IViewComponentResult Invoke()
        {
            var data = _context.Loais.Select(loai => new MenuLoaiVM
            {
                MaLoai = loai.MaLoai,
                TenLoai = loai.TenLoai,
                SoLuong = loai.HangHoas.Count
            }).OrderBy(loai => loai.TenLoai);
            return View(data);
        }
    }
}
