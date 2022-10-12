using FinalLayiheBackend.Data;
using FinalLayiheBackend.Dtos.SaleDtos;
using FinalLayiheBackend.Helpers;
using FinalLayiheBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSaleController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public AdminSaleController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("AllOrder")]
        [Authorize]
        public IActionResult GetAllOrder()
        {

         

            List<AdminSaleReturnDto> adminSaleReturnDto = _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductPhotos)
                .Include(x => x.AppUser)
                .Select(x => new AdminSaleReturnDto
                {
                    Id = x.Id,
                    Date = x.CreatedAt.ToString("MM/dd/yyyy HH:mm"),
                    Mobile = x.Mobile,
                    OrderStatus = x.OrderStatus,
                    ProductPhotos = x.OrderItems.SelectMany(x => x.Product.ProductPhotos).ToList(),
                    Total = x.OrderItems.Select(x => x.Total).Sum(),
                    FirstName = x.AppUser.Name,
                    Address = x.Address,
                    LastName = x.AppUser.Surname,
                    Cash = x.Cash,
                    Note = x.Note,
                    City=x.City,
                  
                    
                })
                .ToList();

            return Ok(adminSaleReturnDto);
        }
        [HttpGet("OrderItem")]
        [Authorize]
        public IActionResult GetAllOrderItem(int orderId)
        {
            List<OrderItemReturnDto> orderItemReturnDto = _context.OrderItems
                 .Where(x => x.OrderId == orderId)
                 .Include(x => x.Product)
                 .Select(x => new OrderItemReturnDto
                 {
                     Id = x.Id,
                     Name = x.Product.Name,
                     Color = x.Product.Color,
                     Count = x.Count,
                     Total = x.Total,
                     Photo = x.Product.ProductPhotos.FirstOrDefault(p => p.IsMain)
                 }).ToList();

            return Ok(orderItemReturnDto);
        }
        [HttpPut]
        [Authorize]
        public IActionResult UpdateOrderStatus(int orderId, int orderStatus)
        {
            Order order = _context.Orders.FirstOrDefault(x => x.Id == orderId);
            order.OrderStatus = (OrderStatus)orderStatus;
            _context.SaveChanges();
            return StatusCode(200);
        }
    }
}
