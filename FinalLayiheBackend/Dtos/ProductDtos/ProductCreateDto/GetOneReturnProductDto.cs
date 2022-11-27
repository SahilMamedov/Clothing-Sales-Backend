using FinalLayiheBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Dtos.ProductDtos.ProductCreateDto
{
    public class GetOneReturnProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string isMainImage { get; set; }
        public double DiscountPrice { get; set; }
        public string Typename { get; set; }
        public string Color { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public Boolean Trending { get; set; }
        public List<Size> size { get; set; }
        public List<ProductPhoto> ProductPhotos { get; set; }
    }
}
