using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static API.Model.Enums;

namespace API.Entities
{
    public class OrderItem
    {
        public string OrderNumber { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public string Price { get; set; }
        public string Weight { get; set; }
        public bool Fufilled { get; set; }
        public bool Purchased { get; set; }
        public string PaymentMethod { get; set; } //this would have been better as an enum
        public bool Preparable { get; set; }
        public bool WaitingForPayment { get; set; }
        public int Quantity { get; set; }        
        public bool Collected { get; set; }
        public string PhoneNumber { get; set; }
        public string Category { get; set; } = "";
        public DateTime OrderDateTime { get; set; }
        public string User { get; set; }
        public int PrepTime { get; set; }
        public List<string> Chefs { get; set; }

        //New Addidtions Before Launch
        public string Flavour { get; set; }
        public string MeatTemperature { get; set; }
        public List<string> Sauces { get; set; }
        public string SubCategory { get; set; } = "";
    }
}
