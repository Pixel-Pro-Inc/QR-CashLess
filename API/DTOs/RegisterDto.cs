using System.Collections.Generic;

namespace API.DTOs
{
    public class RegisterDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Phonenumber { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string NationalIdentityNumber { get; set; }
        public string Password { get; set; }
        public string Restuarant { get; set; }
        public bool Developer { get; set; }
        public bool SuperUser { get; set; }
        public List<string> branchId { get; set; }
        public bool Admin { get; set; }
    }
}
