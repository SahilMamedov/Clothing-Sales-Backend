using FinalLayiheBackend.Data;
using FinalLayiheBackend.Dtos.PaginationDtos;
using FinalLayiheBackend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class ShopController : ControllerBase
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;

        public ShopController(AppDbContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }



        [HttpGet("getAllBrands")]
        public IActionResult GetBrands()
        {
            List<Brand> brands = _context.Brands.ToList();
            return Ok(brands);
        }

        [HttpGet("getAllCategory")]
        public IActionResult GetCategories()
        {
            List<Category> categories = _context.Categories.ToList();
            return Ok(categories);
        }


        [HttpGet("filter")]
        public async Task<IActionResult> GetProduct([FromQuery] int?[] brandIds,[FromQuery] int?[] categoryIds, double minPrice=0, double maxPrice=100, int page = 1, int size = 12)
        {
            IQueryable<ShopReturnDto> query = _context.Products
                .Include(p => p.ProductPhotos)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => !p.isDeleted
                            && p.Price - (p.Price * p.Discount / 100) >= minPrice
                            && p.Price - (p.Price * p.Discount / 100) <= maxPrice
                            && (brandIds.Length == 0 ? true : brandIds.Contains(p.BrandId))
                            && (categoryIds.Length == 0 ? true : categoryIds.Contains(p.CategoryId))
                            )
                .Select(x => new ShopReturnDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Discount = x.Discount,
                    DiscountPrice = x.Price - (x.Price * x.Discount / 100),
                    Brand=x.Brand,
                    



                    ProductPhotos = x.ProductPhotos.Select(x => new ProductPhoto
                    {

                        
                        Path = x.Path,
                        IsMain = x.IsMain

                    }).ToList()
                    
                });


            var totalCount = await query.CountAsync();

            //query = orderBy == 0 ? query.OrderBy(p => p.NewPrice) : query.OrderByDescending(p => p.NewPrice);


            var result = await query.Skip((page - 1) * size).Take(size).ToListAsync();
         
            return Ok(new { totalCount, result});

        }








        [HttpPost]
        public IActionResult SearchProduct(string search)
        {
            if (search == null)
            {
                return BadRequest();
            }
            List<Product> result = _context.Products
                .Include(p => p.ProductPhotos)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .OrderBy(p => p.Id)
                .Where(p => p.Name.ToLower()
                .Contains(search.ToLower()))
                .ToList();

            return Ok(new { result });
        }
    }
}
