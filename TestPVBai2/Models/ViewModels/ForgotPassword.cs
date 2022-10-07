using System.ComponentModel.DataAnnotations;

namespace TestPVBai2.Models.ViewModels
{
    public class ForgotPassword
    {
        [Required, DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
    }
}
