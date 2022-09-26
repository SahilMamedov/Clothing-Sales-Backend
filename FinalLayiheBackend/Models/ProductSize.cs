using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Models
{
    public class ProductSize
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int SIzeId { get; set; }
        public Product Product { get; set; }
        public Size Sizes { get; set; }

    }
}
