using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Aggregates
{
    /// <summary>
    /// This is an aggregate of <see cref="MenuItem"/>s. This provides everything that should be in Menu along with List functionality 
    /// </summary>
    public class Menu:BaseAggregates<MenuItem>
    {
    }
}
