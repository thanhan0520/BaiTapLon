using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace A_D.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        [DisplayName("Tên Đăng Nhập")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [DataType(DataType.Password)]
        [DisplayName("Mật Khẩu")]
        public required string Password { get; set; }

        [DisplayName("Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }
}
