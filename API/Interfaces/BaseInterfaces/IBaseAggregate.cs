﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    /// <summary>
    /// This is a class that will define what all aggregates should have, eg Order.
    /// All Aggregates have the ability of <see cref="List{T}"/> as it inherits from it
    /// </summary>
    public interface IBaseAggregate: IBaseEntity
    {
    }
}