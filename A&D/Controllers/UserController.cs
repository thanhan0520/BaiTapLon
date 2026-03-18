using Microsoft.AspNetCore.Mvc;
using A_D.Models;
using System.IO;

namespace A_D.Controllers
{
    public class UserController : Controller
    {
        private readonly QuanLiBanSuaContext _dbContext;

        public UserController(QuanLiBanSuaContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserRole", user.Role ?? "User");

                    // Kiểm tra role - xóa khoảng trắng và chuyển thành chữ thường
                    string userRole = (user.Role ?? "").Trim().ToLower();

                    // Nếu là admin thì redirect vào admin area
                    if (userRole == "admin")
                    {
                        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile");
                        if (Directory.Exists(uploadsDir))
                        {
                            var adminImage = Directory.GetFiles(uploadsDir, $"admin_{user.UserId}.*").FirstOrDefault();
                            if (!string.IsNullOrWhiteSpace(adminImage))
                            {
                                var fileName = Path.GetFileName(adminImage);
                                HttpContext.Session.SetString("AdminProfileImage", $"/uploads/profile/{fileName}");
                            }
                        }

                        TempData["SuccessMessage"] = "Đăng nhập Admin thành công!";
                        // Dùng Redirect() thay vì RedirectToAction() để chắc chắn
                        return Redirect("/Administrator/Home/Index");
                    }

                    TempData["SuccessMessage"] = "Đăng nhập thành công!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                };
                var temp = _dbContext.Users.FirstOrDefault(u => u.UserName == model.UserName || u.Email == model.Email);
                if(temp == null)
                {
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();
                    TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }
                else {
                    TempData["FailMessage"] = "Tài khoản đã tồn tại, hãy đăng nhập";
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login");
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }
    }
}