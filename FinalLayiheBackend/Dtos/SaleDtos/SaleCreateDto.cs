using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Dtos.SaleDtos
{
    public class SaleCreateDto
    {
        public string Address { get; set; }
        public string Apartment { get; set; }
        public string Note { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string City { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public bool Cash { get; set; }
    }
}
