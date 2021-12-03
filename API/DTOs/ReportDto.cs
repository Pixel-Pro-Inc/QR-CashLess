using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ReportDto
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string branchId { get; set; }
        public string invoice { get; set; }
        public string paymentMethod { get; set; }
    }
}
