using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public Nullable<DateTime> CreateTime { get; set; }
        public int ProductId { get; set; }
        public string Content { get; set; }
        public AppUser AppUser { get; set; }
        public Product Product { get; set; }
    }
}
