using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static API.Model.Enums;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public bool Developer { get; set; }
        public string Email { get; set; }
        public string NationalIdentityNumber { get; set; }
        public bool Admin { get; set; }
        public bool SuperUser { get; set; }
        public string ResetToken { get; set; }
        public List<string> branchId { get; set; }
        public string Restuarant { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}