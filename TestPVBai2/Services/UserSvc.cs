using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPVBai2.Models;
using TestPVBai2.Models.ViewModels;
using TestPVBai2.Helpers;

namespace TestPVBai2.Services
{
    public interface IUser
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<User> LoginAsync(Login login);
        Task<bool> ChangePassAsync(ChangePassWord changePass);
        Task<bool> isPass(string email, string pass);
        Task<bool> isEmail(string email);
        Task<int> AddUserAsync(User User);
        Task<int> UpdateUserAsync(UpdateUser putUser);
        Task<bool> CreateOrUpdateOTPAsync(OTP oTP);//tạo mã otp hoặc cập nhật mã otp mới cho email đã tồn tại
        Task<bool> ConfirmOTPAsync(string email, string otp);//xác nhận OTP sau khi nhập mã OTP
        Task<bool> QuenMatKhauAsync(CreateNewPass quenMatKhau);//đổi mật khẩu mới khi chọn chức năng quên mật khẩu
        Task<bool> DeleteUserAsync(int id_User);
    }
    public class UserSvc:IUser
    {
        private readonly DataContext _context;
        public UserSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<User> LoginAsync(Login login)
        {
            var pass = HashPassword.Mahoa(login.UserPassword);
            var user = await _context.Users
                .Where(x=>x.UserEmail==login.UserEmail && x.UserPassword==pass).FirstOrDefaultAsync();
            if (user != null)
                return user;
            else
                return null;
        }
        public async Task<bool> ChangePassAsync(ChangePassWord changePass)
        {
            bool result = false;
            try
            {
                User user = await _context.Users.Where(x => x.UserEmail == changePass.UserEmail).FirstOrDefaultAsync();
                user.UserPassword = HashPassword.Mahoa(changePass.newPassword);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch { }
            return result;
        }
        public async Task<bool> isPass(string email, string pass)
        {
            bool ret = false;
            try
            {
                User user = await _context.Users.Where(x => x.UserEmail == email && x.UserPassword == pass)
                    .FirstOrDefaultAsync();
                if (user != null)
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        public async Task<bool> isEmail(string email)
        {
            bool ret = false;
            try
            {
                User user = await _context.Users.Where(x => x.UserEmail == email).FirstOrDefaultAsync();
                if (user != null)
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        public async Task<int> AddUserAsync(User user)
        {
            int ret = 0;
            try
            {
                user.UserPassword = HashPassword.Mahoa(user.UserPassword);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                ret= user.UserId;
            }
            catch { ret = 0; }
            return ret;
        }
        public async Task<int> UpdateUserAsync(UpdateUser putUser)
        {
            int ret = 0;
            try
            {
                var update = await _context.Users.Where(x => x.UserId == putUser.UserId).FirstOrDefaultAsync();
                update.UserFullName = putUser.UserFullName;
                update.UserPhone = putUser.UserPhone;
                update.UserBirthday = putUser.UserBirthday;
                update.UserGender = putUser.UserGender;
                _context.Users.Update(update);
                await _context.SaveChangesAsync();
                ret = update.UserId;
            }
            catch { ret = 0; }
            return ret;
        }
        public async Task<bool> CreateOrUpdateOTPAsync(OTP oTP)
        {
            bool result = false;
            try
            {
                OTP otp = new OTP();
                otp = await _context.OTPs.Where(x => x.UserEmail == oTP.UserEmail).FirstOrDefaultAsync();
                if (otp != null)
                {
                    otp.Code_OTP = oTP.Code_OTP;
                    otp.ExpiredAt = oTP.ExpiredAt;
                    otp.isUse = false;
                    _context.OTPs.Update(otp);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    await _context.OTPs.AddAsync(oTP);
                    await _context.SaveChangesAsync();
                }
                SendEmail.Send(oTP.UserEmail, oTP.Code_OTP);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public async Task<bool> ConfirmOTPAsync(string email, string OTP)
        {
            bool result = false;
            try
            {
                OTP otp = new OTP();
                otp = await _context.OTPs.Where(x => x.UserEmail == email && x.Code_OTP == OTP && x.isUse == false
                                && DateTime.Now < x.ExpiredAt).FirstOrDefaultAsync();
                if (otp != null)
                {
                    otp.isUse = true;
                    _context.OTPs.Update(otp);
                    await _context.SaveChangesAsync();
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            catch
            {
                result = false;
            }
            return result;
        }
        public async Task<bool> QuenMatKhauAsync(CreateNewPass quenMatKhau)
        {
            bool result = false;
            try
            {
                User User = await _context.Users.Where(x => x.UserEmail == quenMatKhau.UserEmail).FirstOrDefaultAsync();
                User.UserPassword = HashPassword.Mahoa(quenMatKhau.NewPass);
                _context.Users.Update(User);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch { }
            return result;
        }
        public async Task<User> GetUserAsync(int id)
        {
            return await _context.Users.Where(x => x.UserId == id)
                          .FirstOrDefaultAsync(); ;
        }
        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<bool> DeleteUserAsync(int id_User)
        {
            bool ret = false;
            try
            {
                var user = await _context.Users.Where(x => x.UserId == id_User).FirstOrDefaultAsync();
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }
    }
}
