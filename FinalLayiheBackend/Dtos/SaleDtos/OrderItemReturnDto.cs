using FinalLayiheBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Dtos.SaleDtos
{
    public class OrderItemReturnDto
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public List<ProductPhoto> ProductPhotos { get; set; }

    }
}
