using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public bool Admin { get; set; }
        public bool Developer { get; set; }
        public bool SuperUser { get; set; }
        public List<string> branchId { get; set; }
        //[Required]
        public string Restuarant { get; set; }
    }
}
