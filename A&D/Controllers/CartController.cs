using A_D.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace A_D.Controllers
{
    public class CartController : Controller
    {
        private readonly QuanLiBanSuaContext _dbContext;
        public CartController(QuanLiBanSuaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult CartView()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["FailMessage"] = "Hãy đăng nhập để xem giỏ hàng!";
                return RedirectToAction("Login", "User");
            }
            var cart = _dbContext.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Milk).FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId.Value,
                    CartItems = new List<CartItem>()
                };

                _dbContext.Carts.Add(cart);
                _dbContext.SaveChanges();
            }
            return View(cart);
        }
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["FailMessage"] = "Bạn cần đăng nhập!";
                return RedirectToAction("Login", "User");
            }

            var product = _dbContext.Milk.FirstOrDefault(m => m.MilkId == id);
            if (product == null) return NotFound();

            var cart = _dbContext.Carts.FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId.Value
                };
                _dbContext.Carts.Add(cart);
                _dbContext.SaveChanges();
            }

            var item = _dbContext.CartItems
                .FirstOrDefault(i => i.CartId == cart.CartId && i.MilkId == id);

            if (item == null)
            {
                var newItem = new CartItem
                {
                    CartId = cart.CartId,
                    MilkId = id,
                    Quantity = 1
                };

                _dbContext.CartItems.Add(newItem);
            }
            else
            {
                item.Quantity += 1;
                _dbContext.CartItems.Update(item);
            }

            _dbContext.SaveChanges();

            return RedirectToAction("CartView");
        }
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["FailMessage"] = "Bạn cần đăng nhập!";
                return RedirectToAction("Login", "User");
            }

            var cart = _dbContext.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null) return RedirectToAction("CartView");

            var item = _dbContext.CartItems
                .FirstOrDefault(i => i.CartId == cart.CartId && i.MilkId == id);

            if (item != null)
            {
                _dbContext.CartItems.Remove(item);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("CartView");
        }
        [HttpPost]
        public IActionResult IncreaseQuantity(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "User");

            var cart = _dbContext.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null) return RedirectToAction("CartView");

            var item = _dbContext.CartItems
                .FirstOrDefault(i => i.CartId == cart.CartId && i.MilkId == id);

            if (item != null)
            {
                item.Quantity++;
                _dbContext.SaveChanges();
            }

            return RedirectToAction("CartView");
        }
        [HttpPost]
        public IActionResult DecreaseQuantity(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "User");

            var cart = _dbContext.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null) return RedirectToAction("CartView");

            var item = _dbContext.CartItems
                .FirstOrDefault(i => i.CartId == cart.CartId && i.MilkId == id);

            if (item != null && item.Quantity > 1)
            {
                item.Quantity--;
                _dbContext.SaveChanges();
            }

            return RedirectToAction("CartView");
        }
        [HttpPost]
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["FailMessage"] = "Bạn cần đăng nhập!";
                return RedirectToAction("Login", "User");
            }

            var cart = _dbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                TempData["FailMessage"] = "Giỏ hàng trống!";
                return RedirectToAction("CartView");
            }

            _dbContext.CartItems.RemoveRange(cart.CartItems);

            _dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Thanh toán thành công!";

            return RedirectToAction("CartView");
        }
    }
}
