using System;

namespace API.Core.Entities
{
    public class ClosingTime
    {
        public DateTime EffectiveDate { get; set; }
        public DateTime ClosingTimeToday { get; set; }
    }
}
