using Microsoft.AspNetCore.Mvc;
using A_D.Models;

namespace A_D.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class UserController : Controller
    {
        private readonly QuanLiBanSuaContext _dbContext;

        public UserController(QuanLiBanSuaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var users = _dbContext.Users.ToList();
            return View(users);
        }

        public IActionResult Details(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateRole(int id, string role)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return NotFound();

            user.Role = role;
            _dbContext.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return NotFound();

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
