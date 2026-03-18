using System.Diagnostics;
using A_D.Models;
using Microsoft.AspNetCore.Mvc;

namespace A_D.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuanLiBanSuaContext _dbContext;
        public HomeController(QuanLiBanSuaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Milk> milk = _dbContext.Milk.ToList();
            return View(milk);
        }
        public IActionResult Search(string keyword)
        {
            var result = _dbContext.Milk
                .Where(m => m.MilkName.Contains(keyword))
                .ToList();

            return View("Index", result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
