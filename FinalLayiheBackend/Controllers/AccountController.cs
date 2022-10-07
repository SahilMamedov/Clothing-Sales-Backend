using FinalLayiheBackend.Data;
using FinalLayiheBackend.Dtos.AccountDtos;
using FinalLayiheBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static FinalLayiheBackend.Helpers.Helper;

namespace FinalLayiheBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController:ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]RegisterDto registerDto)
        {
            AppUser user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
            {
                return BadRequest("Bu email artiq movcuddur");
            }
            user = new AppUser
            {
                UserName = registerDto.Email,
                Name = registerDto.Name.ToLower(),
                Email = registerDto.Email,
                Surname = registerDto.Surname.ToLower(),
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    return BadRequest(item.Description);
                }

            }
            result = await _userManager.AddToRoleAsync(user, UserRoles.Member.ToString());
            return StatusCode(200);
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
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim("Name", user.Name));
            claims.Add(new Claim("Surname", user.Surname));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim("Email", user.Email));
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                claims.Add(new Claim("Role", item));
            }
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
        [HttpGet("users")]

        public IActionResult GetUser()
        {
            List<AppUser> appUsers = _context.Users.ToList();
            return Ok(appUsers);

        }

        [HttpGet]
        public async Task CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            }
        }
    }
}
