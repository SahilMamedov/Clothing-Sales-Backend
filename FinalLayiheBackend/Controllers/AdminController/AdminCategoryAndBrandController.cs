using FinalLayiheBackend.Data;
using FinalLayiheBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Controllers.AdminController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminCategoryAndBrandController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminCategoryAndBrandController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetAllBrandAndCategory()
        {
            List<Category> category = _context.Categories.ToList();
            List<Brand> brand = _context.Brands.ToList();
            return Ok(new { category, brand });
        }
        [HttpPost("createCategory")]
        [Authorize]
        public async Task<IActionResult> Create(Category category)
        {

            bool NameCategory = _context.Categories.Any(c => c.Name == category.Name);
            if (NameCategory)
            {
                return BadRequest("Error: This Category is already available");

            }
            Category newCategory = new Category
            {
                Name=category.Name
            };
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();


            return Ok();
        }

        [HttpPut("categoryUpdate")]
        [Authorize]
        public IActionResult CategoryUpdate(Category category)
        {

            Category dbCategory = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            Category dbCategoryName = _context.Categories.FirstOrDefault(c => c.Name.ToLower() == category.Name.ToLower());
            if (dbCategoryName != null)
            {
                if (dbCategoryName.Name != dbCategory.Name)
                {
                    return BadRequest("Error: This Category is already available");
                }
            }
            dbCategory.Name = category.Name;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("removeCategory/{id}")]
        [Authorize]
        public IActionResult Delete(int? id)
        {
            Category dbCategory = _context.Categories.FirstOrDefault(c => c.Id == id);

            _context.Categories.Remove(dbCategory);
            _context.SaveChanges();
            return Ok();
        }


        [HttpPost("createBrand")]
        [Authorize]
        public async Task<IActionResult> CreateBrand(Brand brand)
        {

            bool existNameBrand = _context.Brands.Any(c => c.Name == brand.Name);
            if (existNameBrand)
            {
                return BadRequest("Error This Brand is already available");

            }
            Brand newBrand = new Brand
            {
                Name = brand.Name,

            };
            await _context.Brands.AddAsync(newBrand);
            await _context.SaveChangesAsync();


            return Ok();
        }

        [HttpPut("brandUpdate")]
        [Authorize]
        public IActionResult brandUpdate(Brand brand)
        {

            Brand dbBrand = _context.Brands.FirstOrDefault(c => c.Id == brand.Id);
            Brand dbBrandName = _context.Brands.FirstOrDefault(c => c.Name.ToLower() == brand.Name.ToLower());
            if (dbBrandName != null)
            {
                if (dbBrandName.Name != dbBrand.Name)
                {
                    return BadRequest("Error: This Brand is already available");
                }
            }
            dbBrand.Name = brand.Name;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("removeBrand/{id}")]
        [Authorize]
        public IActionResult DeleteBrand(int? id)
        {
            Brand dbBrand = _context.Brands.FirstOrDefault(c => c.Id == id);

            _context.Brands.Remove(dbBrand);
            _context.SaveChanges();
            return Ok();
        }
    }
}
