using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestPVBai2.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required, Column(TypeName = "nvarchar(100)")]
        public string UserFullName { get; set; }
        [Required, Column(TypeName = "nvarchar(40)"), DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [Required,Column(TypeName ="varchar(15)"),DataType(DataType.PhoneNumber)]
        public string UserPhone { get; set; }
        [Required,Column(TypeName ="date")]
        public DateTime UserBirthday { get; set; }
        [Required]
        public bool UserGender { get; set; }
        [Required(ErrorMessage = "Enter user password")]
        [Column(TypeName = "varchar(50)"), MinLength(6, ErrorMessage = "Password to 6-12 characters")]
        public string UserPassword { get; set; }
        [Required(ErrorMessage = "Enter retype password")]
        [Column(TypeName = "varchar(50)"), MinLength(6, ErrorMessage = "Password to 6-12 characters")]
        [DataType(DataType.Password)]
        [Compare("UserPassword", ErrorMessage = "Password does not match!!!")]
        [NotMapped]
        public string RetypePassWord { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime UserCreatedAt { get; set; }
    }
}
