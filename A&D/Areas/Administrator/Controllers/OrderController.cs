using Microsoft.AspNetCore.Mvc;
using A_D.Models;
using Microsoft.EntityFrameworkCore;

namespace A_D.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class OrderController : Controller
    {
        private readonly QuanLiBanSuaContext _dbContext;

        public OrderController(QuanLiBanSuaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var orders = _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ToList();
            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var order = _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Milk)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null) return NotFound();

            order.OrderStatus = status;
            _dbContext.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }
    }
}
