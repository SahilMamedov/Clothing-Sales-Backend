using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Models
{
    public class Size
    {
        public int Id { get; set; }
        public string Sizes { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
    }
}
