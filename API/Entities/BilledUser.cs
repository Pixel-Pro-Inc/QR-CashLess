﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    /// <summary>
    /// This is a User who is expected to be Billed at the end of every month
    /// </summary>
    /// <remarks>
    /// This will either be Managers or whoever is representing the franchansiee
    /// </remarks>
    public class BilledUser:AppUser
    {
        public DateTime LastPaidDate { get; set; }

        // The assumption is the regardless of how many branches you have they we will all be paid at the same time so one date is enough
        public DateTime DuePaymentDate { get; set; }

        /// <summary>
        /// This is a list of the Branches that the user will be charged for and the Current Charge expected from it.
        /// <remarks>
        /// The Key is the Branchid, the double is the CurrentCharge for that branch
        /// </remarks>
        /// </summary>
        public Dictionary<string, double> BilledBranchIds { get; set; }

    }
}
