using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static API.Model.Enums;

namespace API.Entities
{
    public class AppUser:BaseEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool Developer { get; set; }
        public bool Admin { get; set; }
        public bool SuperUser { get; set; }
        public List<string> branchId { get; set; }
        public string Restuarant { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}