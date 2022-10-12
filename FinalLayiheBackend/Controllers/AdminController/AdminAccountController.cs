using FinalLayiheBackend.Data;
using FinalLayiheBackend.Dtos.AccountDtos;
using FinalLayiheBackend.Helpers;
using FinalLayiheBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        public AdminAccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]LoginDto loginDto)
        {
            AppUser user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return NotFound("İstifadəçi tapılmadı");
            }
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return BadRequest("Xəta baş verib: The username or password you entered is incorrect. Please check the username, re-type the password, and try again.");
            }
            List<Claim> claims = new List<Claim>();
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(Helper.UserRoles.Admin.ToString()))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                claims.Add(new Claim("Name", user.Name));
                claims.Add(new Claim("Surname", user.Surname));
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                claims.Add(new Claim("Email", user.Email));

                string secreKey = "2ee5d5f7-3dd0asd-4a06asd-a341-7f3cdc1a7f2c";
                SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secreKey));
                SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddHours(50),
                    SigningCredentials = credentials,
                    Audience = "http://localhost:14345/",
                    Issuer = "http://localhost:14345/"

                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token) });
            }

            return BadRequest("Xəta baş verib: Admin deyilsiniz");

        }
    }
}
