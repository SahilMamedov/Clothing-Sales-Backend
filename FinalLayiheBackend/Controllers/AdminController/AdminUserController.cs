using FinalLayiheBackend.Data;
using FinalLayiheBackend.Dtos.UserDtos;
using FinalLayiheBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        public AdminUserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet("getAllUser")]
        [Authorize]
        public async Task<IActionResult> getAllUsers()
        {


            List<AdminReturnUserDto> AllUser = new List<AdminReturnUserDto>();
            List<AppUser> appUsers = _userManager.Users.ToList();
            List<string> role = new List<string>();
            var roles = _roleManager.Roles.ToList();

            foreach (var item in roles)
            {
                role.Add(item.Name);
            }

            foreach (var item in appUsers)
            {
                AdminReturnUserDto adminReturnUser = new AdminReturnUserDto();
                adminReturnUser.userRoles = await _userManager.GetRolesAsync(item);
                adminReturnUser.Id = item.Id;
                adminReturnUser.Surname= item.Surname;
                adminReturnUser.Name = item.Name;
                adminReturnUser.Username = item.UserName;
                adminReturnUser.Email = item.Email;
                AllUser.Add(adminReturnUser);

            }
            return Ok(new { AllUser, role });

        }
        [HttpGet("getAllRole")]
        [Authorize]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpDelete("userRemove")]
        [Authorize]
        public async Task<IActionResult> removeUser(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            List<BasketItem> basketItems = _context.BasketItems.Where(x => x.AppUserId == user.Id).ToList();
            List<Order> orders = _context.Orders.Where(x => x.AppUserId == user.Id).ToList();
            foreach (var item in basketItems)
            {
                _context.BasketItems.Remove(item);
            }
            foreach (var item in orders)
            {
                _context.Orders.Remove(item);
            }
            _context.SaveChanges();
            await _userManager.DeleteAsync(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("updateRole")]
        [Authorize]

        public async Task<IActionResult> Update(AdminUserUpdateDto updateRoleDto)
        {
            AppUser user = await _userManager.FindByIdAsync(updateRoleDto.Id);
            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, updateRoleDto.Role);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("createRole")]
        [Authorize]
        public async Task<IActionResult> Create(string role)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole { Name = role });
            return Ok(result);
        }
        [HttpDelete("removeRole")]
        [Authorize]
        public async Task<IActionResult> RemoveRole(string id)
        {
            var role = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
            await _roleManager.DeleteAsync(role);
            _context.SaveChanges();
            return Ok();
        }
    }
}
