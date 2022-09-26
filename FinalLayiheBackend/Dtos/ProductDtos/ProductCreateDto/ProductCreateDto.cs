using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Dtos.ProductDtos.ProductCreateDto
{
    public class ProductCreateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }
        public string Size { get; set; }
        public bool Trending { get; set; }
        public List<IFormFile> ChildPhotos { get; set; }
        public IFormFile Photos { get; set; }
        public string Color { get; set; }
        public string TypeName { get; set; }


        public class ProductCreateDtoValidatio : AbstractValidator<ProductCreateDto>
        {
            public ProductCreateDtoValidatio()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("bosh qoyma");

                
            }
        }
    }
}
