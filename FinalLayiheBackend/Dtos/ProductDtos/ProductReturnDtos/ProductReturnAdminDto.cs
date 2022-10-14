using FinalLayiheBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Dtos.ProductDtos.ProductReturnDtos
{
    public class ProductReturnAdminDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string Typename { get; set; }
        public string Color { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public ProductPhoto Photo { get; set; }
        public Boolean Trending { get; set; }
    }
   
}
