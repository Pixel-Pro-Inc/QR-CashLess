using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
        public string Reference { get; set; }
        public bool Fufilled { get; set; }
        public bool Purchased { get; set; }
        public bool CalledForBill { get; set; }
        public int Quantity { get; set; }
        public DateTime DateOrdered { get; set; }
    }
}
