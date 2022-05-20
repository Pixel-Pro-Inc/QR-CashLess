using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    /// <summary>
    /// This is a Dto that contains all the information needed to find process and send Bills the the franchisee representitives
    /// </summary>
    public class BillInfoDto
    {
        // We expect any kind of user to come through here
        public AppUser User { get; set; }
        public DateTime Date { get; set; }

    }
}
