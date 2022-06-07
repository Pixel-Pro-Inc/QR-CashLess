using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        //[Required]
        public string Restuarant { get; set; }
        public bool Developer { get; set; }
        public bool SuperUser { get; set; }
        public List<string> branchId { get; set; }
        public bool Admin { get; set; }

        // UPDATE: This is so the AdminUser trait can be set if admin is true
        public DateTime DuePaymentDate { get; set; }
    }
}
