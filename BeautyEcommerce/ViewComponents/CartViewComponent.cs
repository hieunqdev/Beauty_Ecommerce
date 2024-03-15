using BeautyEcommerce.Helpers;
using BeautyEcommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BeautyEcommerce.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

            return View("CartPanel", new CartModel
            {
                Quantity = cart.Sum(item => item.SoLuong),
                Total = cart.Sum(item => item.ThanhTien)
            });
        }
    }
}
