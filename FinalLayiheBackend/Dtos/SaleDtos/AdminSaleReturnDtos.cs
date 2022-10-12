using FinalLayiheBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Dtos.SaleDtos
{
    public class AdminSaleReturnDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string Mobile { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Total { get; set; }
        public string Address { get; set; }
        public bool Cash { get; set; }
        public string Note { get; set; }
        public string City { get; set; }

        public List<ProductPhoto> ProductPhotos { get; set; }
    }
}
