using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLayiheBackend.Dtos.UserDtos
{
    public class AdminReturnUserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public IList<string> userRoles { get; set; }
        public string Username { get; set; }
    }
}
