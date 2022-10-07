using System.ComponentModel.DataAnnotations;

namespace TestPVBai2.Models.ViewModels
{
    public class CreateNewPass
    {
        [Required(ErrorMessage = "Bạn cần nhập email")]
        public string UserEmail { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu mới")]
        [StringLength(12)]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu từ 6 - 12 kí tự")]
        public string NewPass { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bạn cần nhập lại mật khẩu mới")]
        [StringLength(12)]
        [MinLength(6, ErrorMessage = "Mật khẩu từ 6 - 12 kí tự")]
        [Compare("NewPass", ErrorMessage = "Mật khẩu không trùng khớp!!!")]
        public string RetypePass { get; set; }
    }
}
