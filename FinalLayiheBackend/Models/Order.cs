using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string Apartment { get; set; }
        public string Mobile { get; set; }
        public bool Cash { get; set; }
        public string City { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
    public enum OrderStatus
    {
        Pending,
        Accept,
        Reject
    }
}

