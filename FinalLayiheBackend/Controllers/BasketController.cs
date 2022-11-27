using FinalLayiheBackend.Data;
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
    public class BasketController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public BasketController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetBasketItems()
        {
            double total = 0;

            string UserToken = HttpContext.Request.Headers["Authorization"].ToString();
            var userId = Helper.DecodeToken(UserToken);
            var obj = new object();


            List<BasketItem> baskets = _context.BasketItems
                .Include(b => b.Product)
                .Include(u => u.AppUser)
                .Where(b => b.AppUserId == userId && !b.Product.isDeleted)
                .ToList();
            foreach (var item in baskets)
            {
                total += item.Sum;
            }
            obj = new
            {
                basketItems = baskets,
                total = total,

            };

            return Ok(obj);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddItem(int productId, int count)
        {
            double total = 0;
            var obj = new object();
            Product dbProduct = _context.Products
                .Include(p => p.ProductPhotos)
                .FirstOrDefault(p => p.Id == productId);

            ProductPhoto productImage = dbProduct.ProductPhotos.FirstOrDefault(p => p.ProductId == productId && p.IsMain);

            BasketItem basketItem = new BasketItem();


            string UserToken = HttpContext.Request.Headers["Authorization"].ToString();
            var userId = Helper.DecodeToken(UserToken);
            BasketItem isExist = _context.BasketItems.Include(b => b.Product).FirstOrDefault(b => b.ProductId == dbProduct.Id && b.AppUserId == userId);
            if (isExist == null)
            {
                basketItem.ProductId = dbProduct.Id;
                basketItem.Price = dbProduct.Price - (dbProduct.Discount * dbProduct.Price) / 100;
                basketItem.AppUserId = userId;
                basketItem.Product = dbProduct;
                basketItem.Count = count;
                basketItem.Path = productImage.Path;
                basketItem.Sum = basketItem.Count * basketItem.Price;
                await _context.AddAsync(basketItem);
            }
            else
            {


                isExist.Count = count;
                    isExist.Sum = isExist.Count * isExist.Price;
                

            }


            await _context.SaveChangesAsync();
            List<BasketItem> basketItems = _context.BasketItems.Include(p => p.Product).Where(b => b.AppUserId == userId).ToList();


            foreach (var item in basketItems)
            {

                total += item.Sum;
               
            }
            obj = new
            {
                basketItems = basketItems,
                total = total,

            };


            return Ok(obj);
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Remove(int? id)
        {

            double total = 0;
            Product dbProduct = _context.Products.Include(p => p.ProductPhotos).FirstOrDefault(p => p.Id == id);
            string UserToken = HttpContext.Request.Headers["Authorization"].ToString();
            var userId = Helper.DecodeToken(UserToken);
            BasketItem isExist = _context.BasketItems.Include(p => p.Product).FirstOrDefault(b => b.ProductId == dbProduct.Id && b.AppUserId == userId);
            if (isExist != null)
            {
                _context.BasketItems.Remove(isExist);

            }
            await _context.SaveChangesAsync();
            List<BasketItem> basketItems = _context.BasketItems.Where(b => b.AppUserId == userId).ToList();

            foreach (var item in basketItems)
            {
                total += item.Sum;

            }

            return Ok(new { total });

        }
    }
}
