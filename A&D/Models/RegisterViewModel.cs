using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace A_D.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Không được trống.")]
        [DisplayName("Tên Đăng Nhập")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email không được trống.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [DisplayName("Email")]
        public required string Email { get; set; }


        [Required(ErrorMessage = "Vui lòng điền mật khẩu.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 20 ký tự.")]
        [DataType(DataType.Password)]
        [DisplayName("Mật Khẩu")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu.")]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp.")]
        [DataType(DataType.Password)]
        [DisplayName("Nhập Lại Mật Khẩu")]
        public required string ConfirmPassword { get; set; }
    }
}