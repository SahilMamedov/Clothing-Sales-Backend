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
            Product product = _context.Products.Include(p => p.ProductPhotos)
                .Include(x => x.Brand)
                .Include(x => x.ProductSizes)
                .ThenInclude(x => x.Sizes)
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id);
            ProductPhoto productImage = product.ProductPhotos.FirstOrDefault(p => p.ProductId == id && p.IsMain);
            GetOneReturnProductDto getOneReturnProductDto = new GetOneReturnProductDto()
            {

                ProductPhotos = product.ProductPhotos.Select(x => new ProductPhoto
                {
                    Path = x.Path,
                    IsMain = x.IsMain,


                }).ToList(),

                size = product.ProductSizes.Select(s => new Size
                {

                    Sizes = s.Sizes.Sizes,
                    Id = s.SIzeId

                }).ToList(),

                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Discount = product.Discount,
                Typename = product.TypeName,
                Trending = product.Trending,
                isMainImage= productImage.Path,
                Brand = product.Brand,
                Category = product.Category,
                Color = product.Color

            };


            return Ok(getOneReturnProductDto);
        }





        [HttpGet("similarProducts")]
        public IActionResult GetAllSimilarProduct(int categoryId)
        {
            IQueryable<GetOneReturnProductDto> query = _context.Products
               .Include(p => p.ProductPhotos)
               .Include(p => p.Brand)
               .Include(p => p.Category)
               .Include(p=>p.ProductSizes)
               .ThenInclude(p=>p.Sizes)
               .Where(x => x.CategoryId == categoryId && !x.isDeleted)
               .Select(x => new GetOneReturnProductDto
               {
                   Id = x.Id,
                   Name = x.Name,
                   Price = x.Price,
                   Discount = x.Discount,
                   Typename = x.TypeName,
                   Trending = x.Trending,
                   Brand = x.Brand,
                   Category = x.Category,
                   
                   Color = x.Color,
                   DiscountPrice = x.Price - (x.Price * x.Discount / 100),




    

            ProductPhotos = x.ProductPhotos.Select(x => new ProductPhoto
                    {
                        Path = x.Path,
                        IsMain = x.IsMain,


                    }).ToList(),

                   size = x.ProductSizes.Select(s => new Size
                   {

                       Sizes = s.Sizes.Sizes,
                       Id = s.SIzeId

                   }).ToList(),



               });
            var result = query.ToList();
            return Ok(result);
        }

        [HttpGet("newArrivalGoods")]
        public IActionResult GetNewArrivalProduct()
        {
            var from = DateTime.UtcNow.AddDays(-10);
            IQueryable<ProductReturnDto> query = _context.Products

                 .Include(p => p.Brand)
                 .Include(p=>p.ProductPhotos)
                .Where(p => p.CreatedDate >= from)

              .Select(x => new ProductReturnDto
              {
                  Id = x.Id,
                  Name = x.Name,
                  Price = x.Price,
                  Discount = x.Discount,
                  DiscountPrice = x.Price - (x.Price * x.Discount / 100),
                  Brand = x.Brand,
                  ProductPhotos = x.ProductPhotos.Select(x => new ProductPhoto
                  {
                      Path = x.Path,
                      IsMain = x.IsMain,


                  }).ToList(),

              });
            var result = query.ToList();

            return Ok(result);


        }


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



    }
}
