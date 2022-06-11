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
        public string Restaurant { get; set; }
        public bool Developer { get; set; }
        public bool SuperUser { get; set; }
        public List<string> branchId { get; set; }
        public bool Admin { get; set; }

        // UPDATE: These are in the case that the user is an admin
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DuePaymentDate { get; set; }

    }
}
