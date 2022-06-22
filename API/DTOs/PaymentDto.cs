using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class PaymentDto
    {
        public string Amount { get; set; }
        public string Method { get; set; }
    }
}
