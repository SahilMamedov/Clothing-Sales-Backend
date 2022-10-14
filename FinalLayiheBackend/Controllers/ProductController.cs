using FinalLayiheBackend.Data;
using FinalLayiheBackend.Dtos.ProductDtos.ProductCreateDto;
using FinalLayiheBackend.Dtos.ProductDtos.ProductReturnDtos;
using FinalLayiheBackend.Extentions;
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
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll(string typeName)
        {
            
            List<Product> products = _context.Products.Include(p => p.ProductPhotos).Include(p=>p.ProductSizes).Include(p => p.Brand).Where(p => !p.isDeleted && (typeName != null ? p.TypeName.ToLower()  == typeName.ToLower(): true)).ToList();
            List<ProductReturnDto> productReturnDtos = new List<ProductReturnDto>();
            foreach (var item in products)
            {
                ProductReturnDto productReturnDto = new ProductReturnDto();

                productReturnDto.Id = item.Id;
                productReturnDto.Name = item.Name;
                productReturnDto.Price = item.Price;
                productReturnDto.Brand = item.Brand;
                productReturnDto.Discount = item.Discount;
                productReturnDto.DiscountPrice = item.Price-(item.Price * item.Discount/100);
                productReturnDto.TypeName = item.TypeName;
                productReturnDto.Trending = item.Trending;

              
               
                foreach (var photo in item.ProductPhotos)
                {
                    if (photo.IsMain)
                    {
                        productReturnDto.PhotoPath = photo.Path;
                        productReturnDto.IsMainPhoto = photo.IsMain;
                    }
                   
                };

                productReturnDtos.Add(productReturnDto);
            }

            return Ok(productReturnDtos);
        }
[HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            Product product = _context.Products
                .Include(p=>p.ProductPhotos)
                .Include(p=>p.Brand)
                .Include(p=>p.Category)
                .Include(p=>p.ProductSizes)
                .FirstOrDefault(p => p.Id == id && !p.isDeleted);

           


            return Ok(product);
        }

        //[HttpGet("bestSelling")]
        //public IActionResult GetBestSellingProduct()
        //{
        //    IQueryable<ProductReturnDto> query = _context.Ratings
        //      .Include(r => r.Product)
        //        .ThenInclude(p => p.Photos)
        //        .Include(r => r.Product)
        //        .ThenInclude(p => p.Category)

        //        .Where(r => r.Avarge > 1 && !r.Product.isDeleted)
        //      .Select(x => new ProductReturnDto
        //      {
        //          Id = x.ProductId,
        //          Title = x.Product.Title,
        //          Description = x.Product.Description,
        //          NewPrice = x.Product.NewPrice,
        //          OldPrice = x.Product.OldPrice,
        //          CategoryTitle = x.Product.Category.Title,
        //          inStock = x.Product.inStock,
        //          Photos = x.Product.Photos.Select(x => new Photo
        //          {
        //              Id = x.ProductId,
        //              Path = x.Path,
        //              IsMain = x.IsMain

        //          }).ToList()
        //      });
        //    var result = query.ToList();

        //    return Ok(result);
        //}

        //[HttpGet("newArrival")]
        //public IActionResult GetNewArrivalProduct()
        //{
        //    var from = DateTime.UtcNow.AddDays(-20);
        //    IQueryable<ProductReturnDto> query = _context.Products
        //         .Include(p => p.Category)
        //        .Where(p => p.CreatedDate >= from)
        //      .Select(x => new ProductReturnDto
        //      {
        //          Id = x.Id,
        //          Title = x.Title,
        //          Description = x.Description,
        //          NewPrice = x.NewPrice,
        //          OldPrice = x.OldPrice,
        //          CategoryTitle = x.Category.Title,
        //          inStock = x.inStock,
        //          Photos = x.Photos.Select(x => new Photo
        //          {
        //              Id = x.ProductId,
        //              Path = x.Path,
        //              IsMain = x.IsMain

        //          }).ToList()
        //      });
        //    var result = query.ToList();

        //    return Ok(result);


        //}


        [HttpPost("createProduct")]
        public IActionResult Create([FromForm] ProductCreateDto productCreateDto)
        {


            foreach (var item in productCreateDto.ChildPhotos)
            {

                if (item == null)
                {

                    return BadRequest("Bosqoyma");
                }
                if (!item.IsImage())
                {

                    return BadRequest("only Photo");

                }
                if (item.ValidSize(200))
                {
                    return BadRequest("olcu uygun deyil");
                }


            }

            if (productCreateDto.Photos == null)
            {

                return BadRequest("Bosqoyma");
            }
            if (!productCreateDto.Photos.IsImage())
            {

                return BadRequest("only Photo");

            }
            if (productCreateDto.Photos.ValidSize(200))
            {
                return BadRequest("olcu uygun deyil");
            }

            List<ProductPhoto> photos = new List<ProductPhoto>();

            foreach (var item in productCreateDto.ChildPhotos)
            {

                ProductPhoto photo = new ProductPhoto
                {
                    
                    Path = item.SaveImage(_env, "img"),
                    IsMain = false
                };
                photos.Add(photo);
            }

            ProductPhoto isMainPhoto = new ProductPhoto
            {
                Path = productCreateDto.Photos.SaveImage(_env, "img"),
                IsMain = true,
            };
            photos.Add(isMainPhoto);



            Product newProduct = new Product
            {
                Name=productCreateDto.Name,
                BrandId=productCreateDto.BrandId,
                CategoryId=productCreateDto.CategoryId,
                Price=productCreateDto.Price,
                Discount=productCreateDto.Discount,
                Trending=productCreateDto.Trending,
                ProductPhotos=photos,
                TypeName=productCreateDto.TypeName,
                Color=productCreateDto.Color,


                //ProductSizes = new List<ProductSize>
                //{
                //    new ProductSize
                //    {
                //        Sizes= new Size
                //        {
                //            Sizes=productCreateDto.Size
                //        }
                //    }
                //},


            };

            _context.Add(newProduct);
            _context.SaveChanges();
            return StatusCode(201);
        }

        //[HttpGet("brandAndCategoryIds")]
        //public IActionResult GetBrandAndCategoryId()
        //{
        //    List<Brand> dbBrands = _context.Brand.Where(b => !b.IsDeleted).ToList();

        //    List<Category> dbCategory = _context.Categories.ToList();

        //    var obj = new
        //    {
        //        Brand = dbBrands,
        //        Category = dbCategory
        //    };
        //    return Ok(obj);
        //}

        //[HttpPut]
        //public IActionResult Update(Product product)
        //{
        //    return StatusCode(200);
        //}



        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    Product p = _context.Products.FirstOrDefault(p => p.Id == id);
        //    if (p == null)
        //    {
        //        return NotFound();
        //    }
        //    p.isDeleted = true;
        //    _context.SaveChanges();
        //    return StatusCode(200);
        //}

    }
}
