﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class SalesDto
    {
        public string OrderNumber { get; set; }
        public string OrderRevenue { get; set; }
        public float SummaryTotal { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }

    }
}
