using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public bool isDeleted { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public bool Trending { get; set; }
        public string TypeName { get; set; }
        public string Color { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public List<Comment> Comments { get; set; }
        public List<ProductPhoto> ProductPhotos { get; set; }
        public int CategoryId { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        public Category Category { get; set; }

    }
}
