using Microsoft.AspNetCore.Mvc;
using A_D.Models;
using Microsoft.EntityFrameworkCore;

namespace A_D.Controllers
{
    public class OrderController : Controller
    {
        private readonly QuanLiBanSuaContext _dbContext;

        public OrderController(QuanLiBanSuaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["FailMessage"] = "Hãy đăng nhập để xem đơn hàng!";
                return RedirectToAction("Login", "User");
            }

            var orders = _dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Milk)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        public IActionResult PlaceOrder()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["FailMessage"] = "Bạn cần đăng nhập!";
                return RedirectToAction("Login", "User");
            }

            var cart = _dbContext.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Milk)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                TempData["FailMessage"] = "Giỏ hàng trống!";
                return RedirectToAction("CartView", "Cart");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                OrderStatus = "Pending",
                TotalAmount = 0
            };

            foreach (var item in cart.CartItems)
            {
                var milk = item.Milk;
                order.OrderItems.Add(new OrderItem
                {
                    MilkId = item.MilkId,
                    Quantity = item.Quantity,
                    Price = milk.Price
                });
                order.TotalAmount += milk.Price * item.Quantity;
            }

            _dbContext.Orders.Add(order);
            _dbContext.CartItems.RemoveRange(cart.CartItems);
            _dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Đơn hàng đã được tạo thành công!";
            return RedirectToAction("MyOrders");
        }

        public IActionResult OrderDetails(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "User");

            var order = _dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Milk)
                .FirstOrDefault(o => o.OrderId == id && o.UserId == userId);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
