using System;
using System.ComponentModel.DataAnnotations;

namespace TestPVBai2.Models.ViewModels
{
    public class UpdateUser
    {
        public int UserId { get; set; }
        [Required]
        public string UserFullName { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public string UserPhone { get; set; }
        [Required]
        public DateTime UserBirthday { get; set; }
        [Required]
        public bool UserGender { get; set; }
    }
}
