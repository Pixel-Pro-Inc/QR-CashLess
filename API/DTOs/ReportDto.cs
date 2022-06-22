using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BranchId { get; set; }
        public string Invoice { get; set; }
        public string PaymentMethod { get; set; }
        //Filter
        public string Category { get; set; }
        public string Name { get; set; }
    }
}
