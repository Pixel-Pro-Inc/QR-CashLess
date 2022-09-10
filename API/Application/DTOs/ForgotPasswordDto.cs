using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
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
