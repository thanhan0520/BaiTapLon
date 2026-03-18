using Microsoft.AspNetCore.Mvc;
using A_D.Models;

namespace A_D.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class HomeController : Controller
    {
        private readonly QuanLiBanSuaContext _dbContext;

        public HomeController(QuanLiBanSuaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var dashboard = new
            {
                TotalProducts = _dbContext.Milk.Count(),
                TotalUsers = _dbContext.Users.Count(),
                TotalOrders = _dbContext.Orders.Count(),
                TotalRevenue = _dbContext.Orders.Where(o => o.OrderStatus == "Completed").Sum(o => o.TotalAmount ?? 0)
            };
            return View(dashboard);
        }
    }
}
