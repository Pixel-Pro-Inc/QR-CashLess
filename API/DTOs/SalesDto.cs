using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class SalesDto
    {
        public List<string> orderNumbers { get; set; }
        public List<float> orderRevenue { get; set; }
        public float SummaryTotal { get; set; }
        public Dictionary<string,float> paymentMethodList { get; set; } // variables are <PaymentMethod, Price>
        public List<string> ItemName { get; set; }
        public List<int> Quantity { get; set; }

    }
}
