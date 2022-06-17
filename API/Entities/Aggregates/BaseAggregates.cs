using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Aggregates
{
    /// <summary>
    /// This is a class that will define what all aggregates should have, especially Order
    /// </summary>
    public class BaseAggregates<T>: List<T>
    {
    }
}
