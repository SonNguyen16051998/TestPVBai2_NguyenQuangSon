using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using TestPVBai2.Models;
using TestPVBai2.Models.ViewModels;
using TestPVBai2.Helpers;
using TestPVBai2.Services;
using Microsoft.AspNetCore.Authorization;

namespace TestPVBai2.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUser _user;
        public UsersController(IUser user)
        {
            _user = user;
        }
        /// <summary>
        /// trả về toàn bộ người dùng 
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("User")]
        public async Task<IActionResult> GetUsersAsync() 
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách người dùng thành công",
                data = await _user.GetUsersAsync()
            });
        }
        /// <summary>
        /// trả về người dùng được chọn
        /// </summary>
        /// <param name="id">mã người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("User")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy người dùng thành công",
                data = await _user.GetUserAsync(id)
            });
        }
        /// <summary>
        /// thêm người dùng.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, ActionName("register")]
        public async Task<IActionResult> PostUserAsync([FromBody] PostUser user)
        {

            if (ModelState.IsValid)
            {
                User addUser = new User()
                {
                    UserFullName=user.UserFullName,
                    UserBirthday=user.UserBirthday,
                    UserEmail=user.UserEmail,
                    UserGender=user.UserGender,
                    UserPhone=user.UserPhone,
                    UserPassword=user.UserPassword,
                    UserCreatedAt=DateTime.Now,
                };
                if (await _user.isEmail(user.UserEmail))
                {
                    return Ok(new
                    {
                        retCode = 0,
                        retText = "Email đã tồn tại",
                        data = ""
                    });
                }
                else
                {
                    int id_User = await _user.AddUserAsync(addUser);
                    if (id_User > 0)
                    {
                        return Ok(new
                        {
                            retCode = 1,
                            retText = "Thêm người dùng thành công",
                            data = await _user.GetUserAsync(id_User)
                        });
                    }
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Dữ liệu không hợp lệ",
                data = ""
            });
        }
        /// <summary>
        /// cập nhật thông tin người dùng
        /// </summary>
        /// <param name="putUser"></param>
        /// <returns></returns>
        [HttpPut, ActionName("User")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUser putUser)
        {
            if (ModelState.IsValid)
            {
                int id_User = await _user.UpdateUserAsync(putUser);
                if (id_User > 0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật thông tin người dùng thành công",
                        data = await _user.GetUserAsync(id_User)
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Cập nhật người dùng thất bại",
                data = ""
            });
        }
        /// <summary>
        /// đổi mật khẩu
        /// </summary>
        /// <param name="changePass">mật khẩu từ 6 đến 30 kí tự</param>
        /// <returns></returns>
        [HttpPut, ActionName("doimatkhau")]
        public async Task<IActionResult> DoiMatKhauAsync([FromBody] ChangePassWord changePass)
        {
            if (ModelState.IsValid)
            {
                if (await _user.isPass(changePass.UserEmail, changePass.UserPassword))
                {
                    await _user.ChangePassAsync(changePass);
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Đổi mật khẩu thành công",
                        data = new
                        {
                            Email = changePass.UserEmail,
                            Password = changePass.newPassword
                        }
                    });
                }
                else
                {
                    return Ok(new
                    {
                        retCode = 0,
                        retText = "Mật khẩu củ không chính xác",
                        data = ""
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Đổi mật khẩu thất bại",
                data = ""
            });
        }
        /// <summary>
        /// chức năng cập nhật mật khẩu khi đã xác nhận mã otp thành công
        /// </summary>
        /// <param name="quenMatKhau"></param>
        /// <returns></returns>
        [HttpPut, ActionName("quenmatkhau")]
        [AllowAnonymous]
        public async Task<IActionResult> QuenMatKhauAsync([FromBody] CreateNewPass quenMatKhau)
        {
            //xác nhận mã OTP thành công cho qua trang cập nhật mật khẩu mới
            if (ModelState.IsValid)
            {
                if (await _user.QuenMatKhauAsync(quenMatKhau))
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật mật khẩu thành công",
                        data = new
                        {
                            Email = quenMatKhau.UserEmail,
                            NewPass = quenMatKhau.NewPass
                        }
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Cập nhật mật khẩu thất bại",
                data = ""
            });
        }
        /// <summary>
        /// chức năng nhận mã OTP khi xác nhận xong capcha và nhập email chính xác. cần nhập email đã có tài khoản
        /// </summary>
        /// <param name="maOTP"></param>
        /// <returns></returns>
        [HttpPut, ActionName("maotp")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrUpdateOTPAsync(ForgotPassword maOTP)
        {
            if (ModelState.IsValid)
            {
                OTP oTP = new OTP()
                {
                    UserEmail = maOTP.UserEmail,
                    Code_OTP = RandomOTP.random(),
                    isUse = false,
                    ExpiredAt = DateTime.Now.AddMinutes(2)
                };
                if (await _user.isEmail(maOTP.UserEmail))
                {
                    if (await _user.CreateOrUpdateOTPAsync(oTP))
                    {
                        return Ok(new
                        {
                            retCode = 1,
                            retText = "Mã otp đã được gửi đến email",
                            data = new
                            {
                                Email = maOTP.UserEmail,
                                OTP = oTP. Code_OTP
                            }
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        retCode = 0,
                        retText = "Email không tồn tại",
                        data = ""
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Dữ liệu không hợp lệ",
                data = ""
            });
        }
        /// <summary>
        /// chức năng xác nhận mã OTP
        /// </summary>
        /// <param name="maOTP"></param>
        /// <returns></returns>
        [HttpPut, ActionName("xacnhanotp")]
        [AllowAnonymous]
        public async Task<IActionResult> XacNhanOTP(ConfirmOTP maOTP)
        {
            if (ModelState.IsValid)
            {
                if (await _user.ConfirmOTPAsync(maOTP.Email, maOTP.OTP))
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Mã OTP chính xác",
                        data = ""
                    });
                }
                else
                {
                    return Ok(new
                    {
                        retCode = 0,
                        retText = "Mã OTP đã hết hạn hoặc đã sử dụng",
                        data = ""
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Dữ liệu không hợp lệ",
                data = ""
            });
        }
        /// <summary>
        /// xóa người dùng
        /// </summary>
        /// <param name="id">mã người dùng</param>
        /// <returns></returns>
        [HttpDelete("{id}"), ActionName("deleteUser")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            if (id > 0)
            {
                if (await _user.DeleteUserAsync(id))
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Xóa người dùng thành công",
                        data = await _user.GetUsersAsync()
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Mã người dùng không tồn tại",
                data = ""
            });
        }
    }
}
