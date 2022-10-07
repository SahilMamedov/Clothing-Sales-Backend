using FinalLayiheBackend.Data;
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


        [HttpGet("{id}")]
        public IActionResult FilterBrand(int id)
        {
            List<Product> products = _context.Products.Where(p => p.BrandId == id).ToList();
            return Ok(products);
        }



        //[HttpPost("search")]
        //public IActionResult SearchProduct(string search)
        //{
        //    if (search == null)
        //    {
        //        return BadRequest();
        //    }
        //    List<Product> result = _context.Products
        //        .Include(p => p.ProductPhotos)
        //        .Include(p => p.Category)
        //        .Include(p => p.ProductColors)
        //        .ThenInclude(p => p.Colors)
        //        .Include(p => p.ProductDetails)
        //        .OrderBy(p => p.Id)
        //        .Where(p => p.Title.ToLower()
        //        .Contains(search.ToLower()))
        //        .ToList();

        //    return Ok(new { result });
        //}
    }
}
