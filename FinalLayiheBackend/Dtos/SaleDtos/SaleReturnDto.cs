using FinalLayiheBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Dtos.SaleDtos
{
    public class SaleReturnDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public double Total { get; set; }

    }
}
