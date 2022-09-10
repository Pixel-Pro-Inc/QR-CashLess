using System.ComponentModel.DataAnnotations;

namespace API.Application.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        //[Required]
        public string Restuarant { get; set; }
        public bool Developer { get; set; }
        public bool Admin { get; set; }
    }
}
