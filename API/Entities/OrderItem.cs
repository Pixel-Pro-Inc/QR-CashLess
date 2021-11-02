using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class OrderItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
        public bool Fufilled { get; set; }
        public bool Purchased { get; set; }
        public int Quantity { get; set; }
        public string OrderNumber { get; set; }
    }
}
