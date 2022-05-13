using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class ClosingTime
    {
        public DateTime EffectiveDate { get; set; }
        public DateTime ClosingTimeToday { get; set; }
    }
}
