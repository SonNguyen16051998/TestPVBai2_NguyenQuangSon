using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestPVBai2.Models.ViewModels;
using TestPVBai2.Services;

namespace TestPVBai2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private IConfiguration _config;
        private readonly IUser _User;
        public LoginController(IConfiguration config, IUser User)
        {
            _config = config;
            _User = User;
        }
        /// <summary>
        /// đăng nhập
        /// </summary>
        /// <param name="login">trả về object login gồm email và mật khẩu</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CusLogin([FromBody] Login login)
        {
            if (ModelState.IsValid)
            {
                var user = await _User.LoginAsync(login);
                if (user != null)
                {
                    var Claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_config["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim("Id",user.UserId.ToString()),
                        new Claim("Name",user.UserFullName),
                        new Claim("Email",user.UserEmail)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                        _config["Jwt:Audience"], Claims, expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn);
                    Token viewToken = new Token() { stringToken = new JwtSecurityTokenHandler().WriteToken(token), user = user };
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Đăng nhập thành công",
                        data = viewToken
                    });
                }
                else
                {
                    return Ok(new
                    {
                        retCode = 0,
                        retText = "Tài khoản hoặc mật khẩu không chính xác",
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
    }
}
