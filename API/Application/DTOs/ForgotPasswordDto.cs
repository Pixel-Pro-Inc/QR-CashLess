using System.ComponentModel.DataAnnotations;

namespace API.Application.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string phoneNumber { get; set; }
        [Required]
        public string ClientURI { get; set; }
    }
}
