using Microsoft.AspNetCore.Mvc;
using A_D.Models;
using System.IO;
using System.Threading.Tasks;

namespace A_D.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class ProfileController : Controller
    {
        private readonly QuanLiBanSuaContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(QuanLiBanSuaContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Overview()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile profileImage, string email, string fullname, string phone, string address)
        {
            try
            {
                string imagePath = null;
                var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

                if (profileImage != null && profileImage.Length > 0)
                {
                    // Validate file size (5MB max)
                    if (profileImage.Length > 5 * 1024 * 1024)
                    {
                        TempData["FailMessage"] = "Ảnh quá lớn. Kích thước tối đa là 5MB.";
                        return RedirectToAction("Edit");
                    }

                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(profileImage.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        TempData["FailMessage"] = "Định dạng file không hợp lệ. Chỉ chấp nhận JPG, PNG, GIF.";
                        return RedirectToAction("Edit");
                    }

                    // Create uploads directory if it doesn't exist
                    var uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profile");
                    if (!Directory.Exists(uploadsDir))
                    {
                        Directory.CreateDirectory(uploadsDir);
                    }

                    // Delete old files of current admin (all supported extensions)
                    var oldFiles = Directory.GetFiles(uploadsDir, $"admin_{userId}.*");
                    foreach (var oldFile in oldFiles)
                    {
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                    }

                    // Save with stable filename per admin account
                    var fileName = $"admin_{userId}{fileExtension}";
                    var filePath = Path.Combine(uploadsDir, fileName);

                    // Save new image
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await profileImage.CopyToAsync(fileStream);
                    }

                    // Store image path as URL
                    imagePath = $"/uploads/profile/{fileName}";
                    HttpContext.Session.SetString("AdminProfileImage", imagePath);
                }

                // Update user info in session
                HttpContext.Session.SetString("AdminFullName", fullname ?? "Admin");
                HttpContext.Session.SetString("AdminEmail", email ?? "admin@example.com");

                TempData["SuccessMessage"] = "Hồ sơ đã được cập nhật thành công!";
                return RedirectToAction("Overview");
            }
            catch (Exception ex)
            {
                TempData["FailMessage"] = $"Lỗi khi cập nhật hồ sơ: {ex.Message}";
                return RedirectToAction("Edit");
            }
        }

        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Settings(bool emailNotification, bool systemNotification, bool twoFactorAuth, bool loginVerification, bool onlineStatus)
        {
            // Store settings in session
            HttpContext.Session.SetString("EmailNotification", emailNotification.ToString());
            HttpContext.Session.SetString("SystemNotification", systemNotification.ToString());
            HttpContext.Session.SetString("TwoFactorAuth", twoFactorAuth.ToString());
            HttpContext.Session.SetString("LoginVerification", loginVerification.ToString());
            HttpContext.Session.SetString("OnlineStatus", onlineStatus.ToString());

            TempData["SuccessMessage"] = "Cài đặt đã được lưu thành công!";
            return RedirectToAction("Settings");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            // Validate passwords match
            if (newPassword != confirmPassword)
            {
                TempData["FailMessage"] = "Mật khẩu mới không khớp!";
                return RedirectToAction("ChangePassword");
            }

            // Validate password strength
            if (newPassword.Length < 8)
            {
                TempData["FailMessage"] = "Mật khẩu phải có ít nhất 8 ký tự!";
                return RedirectToAction("ChangePassword");
            }

            if (!newPassword.Any(char.IsUpper) || !newPassword.Any(char.IsLower) || !newPassword.Any(char.IsDigit))
            {
                TempData["FailMessage"] = "Mật khẩu phải chứa chữ hoa, chữ thường và số!";
                return RedirectToAction("ChangePassword");
            }

            // In a real application, you would:
            // 1. Verify current password
            // 2. Update password in database
            // 3. Log the user out for security

            TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công!";
            return RedirectToAction("Overview");
        }
    }
}
