using System.ComponentModel.DataAnnotations;

namespace TestPVBai2.Models.ViewModels
{
    public class Login
    {
        [Required, DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [Required(ErrorMessage = "Enter user password")]
        [ MinLength(6, ErrorMessage = "Password to 6-12 characters")]
        public string UserPassword { get; set; }
    }
}
