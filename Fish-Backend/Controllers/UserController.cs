using Microsoft.AspNetCore.Mvc;
using Fish_Backend.Data;
using Fish_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Fish_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext context;

        public UserController(AppDbContext context) => this.context = context;

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool checkUser = await context.Users
                .AnyAsync(name => name.username == user.username);
            if (checkUser)
                return BadRequest(new
                {
                    success = false,
                    message = "Tài khoản này đã có người đăng kí"

                });

            bool checkEmail = await context.Users
                .AnyAsync(name => name.email == user.email);
            if (checkEmail)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Trung email"

                });
            }

            try
            {
                // PostgreSQL không chấp nhận DateTime kiểu Local 
                // DateTime dạng UTC.
                user.created_at = DateTime.UtcNow;
                user.money = 50;
                context.Add(user);
                await context.SaveChangesAsync();
                return Ok( new
                {
                    success = true,
                    message = "Đăng kí thành công"
                });
            }
            catch
            {
                return StatusCode(500, "Lỗi hệ thống, vui lòng thử lại sau");
            }
        }
        [HttpGet("Info")]
        public async Task<IActionResult> Info(string user)
        {

            var u = await context.Users
                .FirstOrDefaultAsync(name => name.username == user);
            if (u == null)
                return BadRequest(new
                {
                    success = false,
                    message = "Không thể load tài khoản"
                });
            return Ok(new
            {
                success = true,
                message = "Load tài khoản",
                username = u.username,
                money = u.money,
                id = u.id
            });

        }
        [HttpPut("setting")]
        public async Task<IActionResult> Setting(string account ,string currentPassword, string newPassword)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(name => name.username == account);
            if (user == null)
            {
                return BadRequest("Sai tên tài khoản");
            }
            if (currentPassword != user.password) 
            {
                return Unauthorized("Sai mật khẩu");
            }
            try
            {
                user.password = newPassword;
                await context.SaveChangesAsync();
                return Ok("Thay đổi mật khẩu thành công");
            }
            catch
            {
                return StatusCode(500, "Lỗi");
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string account, string password)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(name => name.username == account);
            if (user == null)
                return BadRequest(new
                {
                    success = false,
                    message = "tài khoản không tồn tại"
                }
                );

            if (user.password != password)
                return Unauthorized(new
                {
                    success = false,
                    message = "Sai mật khẩu"
                }
                );
            try
            {
                return Ok(new
                {
                    success = true,
                    message = "Đăng nhập thành công",        
                });
            }
            catch
            {
                return StatusCode(500, "Lỗi");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUserMoney(int id, int money)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return BadRequest(new
                {
                    success = true,
                    message = "Không tìm thấy tài khoản"
                });
            }
            try
            {
                user.money = money;
                await context.SaveChangesAsync();
                return Ok(new
                {
                    success = true,
                    message = "Thay đổi tài khoản thành công"
                });
            }
            catch
            {
                return StatusCode(500, "Lỗi");
            }
        }
    }
}
