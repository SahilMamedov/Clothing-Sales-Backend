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

namespace FinalLayiheBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public SaleController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaleStart([FromForm] SaleCreateDto saleCreateDto)
        {
            string UserToken = HttpContext.Request.Headers["Authorization"].ToString();
            var userId = Helper.DecodeToken(UserToken);

            List<Product> dbProducts = _context.Products.Include(p => p.ProductPhotos).ToList();
            List<BasketItem> basketItems = _context.BasketItems.Include(b => b.Product).Where(b => b.AppUserId == userId).ToList();
            List<OrderItem> orderItems = new List<OrderItem>();
            AppUser user = await _userManager.FindByIdAsync(userId);

            Order order = new Order();

            order.AppUserId = userId;
            order.AppUser = user;
            order.Address = saleCreateDto.Address;
            order.Note = saleCreateDto.Note;
            order.Apartment = saleCreateDto.Apartment;
            order.Mobile = saleCreateDto.Mobile;
            order.Cash = saleCreateDto.Cash;
            order.City = saleCreateDto.City;
            order.CreatedAt = DateTime.Now;
            order.OrderStatus = OrderStatus.Pending;

            await _context.Orders.AddAsync(order);

            foreach (var item in basketItems)
            {
              
                OrderItem orderItem = new OrderItem()

                {

                    Count = item.Count,
                    Total = item.Sum,
                    Product = item.Product,
                    OrderId = order.Id,
                    Order = order,
                    ProductId = item.ProductId
                };
                await _context.AddAsync(orderItem);
                _context.BasketItems.Remove(item);
            }
            await _context.SaveChangesAsync();

            return Ok(order);
        }
        [HttpGet("getOrder")]
        [Authorize]
        public IActionResult GetOrder(int orderId)
        {
            string UserToken = HttpContext.Request.Headers["Authorization"].ToString();
            var userId = Helper.DecodeToken(UserToken);


            List<Order> orders = _context.Orders
                .Include(u=>u.AppUser)
                .Where(x => x.Id == orderId && x.AppUserId==userId).ToList();
             

            return Ok(orders);
        }

        [HttpGet("getOrderItemAll")]
        [Authorize]
        public IActionResult GetOrderItem(int orderId)
        {
            string UserToken = HttpContext.Request.Headers["Authorization"].ToString();
            var userId = Helper.DecodeToken(UserToken);
            List<OrderItemReturnDto> OrderItemReturnDto = _context.OrderItems
                .Where(x => x.OrderId == orderId)
                .Include(x => x.Product)
                .Select(x => new OrderItemReturnDto
                {
                    Id = x.Id,
                    Name = x.Product.Name,
                    Count = x.Count,
                    Total = x.Total,
                    ProductPhotos = x.Product.ProductPhotos
                }).ToList();

            return Ok(OrderItemReturnDto);
        }
        [HttpGet("getOrderAll")]
        [Authorize]
        public IActionResult GetOrder()
        {
            string UserToken = HttpContext.Request.Headers["Authorization"].ToString();
            var userId = Helper.DecodeToken(UserToken);

            List<SaleReturnDto> saleReturnDto = _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .Where(x => x.AppUserId == userId)
                .Select(x => new SaleReturnDto
                {
                    Id = x.Id,
                    Total = x.OrderItems.Select(i => i.Total).Sum(),
                    Date = x.CreatedAt.ToString("MM/dd/yyyy HH:mm"),
                    OrderStatus = x.OrderStatus
                    
                }).ToList();



            return Ok(saleReturnDto);
        }

    }
}
