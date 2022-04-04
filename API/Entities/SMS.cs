using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class SMS
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public DateTime DateSent { get; set; }
    }
}
