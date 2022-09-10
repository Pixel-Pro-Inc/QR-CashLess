using System.Collections.Generic;

namespace API.Application.DTOs
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
