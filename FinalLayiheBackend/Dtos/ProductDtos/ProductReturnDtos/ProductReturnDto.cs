using FinalLayiheBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Dtos.ProductDtos.ProductReturnDtos
{
    public class ProductReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoPath { get; set; }
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public string TypeName { get; set; }
        public Nullable<double> Discount { get; set; }
        public bool IsMainPhoto { get; set; }
        public bool Trending { get; set; }
        public ProductPhoto Photo { get; set; }
        public Brand Brand { get; set; }
        public List<ProductPhoto> ProductPhotos { get; set; }
    }
}
