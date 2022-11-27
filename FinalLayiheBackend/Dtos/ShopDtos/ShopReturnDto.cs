using FinalLayiheBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Dtos.PaginationDtos
{
    public class ShopReturnDto
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public Nullable<double> DiscountPrice { get; set; }
        public Nullable<double> Discount { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string TypeName { get; set; }
        public int CategoryId { get; set; }
        public Brand Brand { get; set; }
        public List<ProductPhoto> ProductPhotos { get; set; }
    }
}
