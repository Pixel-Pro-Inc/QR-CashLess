using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    /// <summary>
    /// This is a User who is expected to be Billed at the end of every month
    /// 
    /// This will either be Managers or whoever is representing the franchansiee
    /// </summary>
    public class BilledUser:AppUser
    {
        public DateTime LastPaidDate { get; set; }

        // The assumption is the regardless of how many branches you have they we will all be paid at the same time so one date is enough
        public DateTime DuePaymentDate { get; set; }

        /// <summary>
        /// This is a list of the Branches that the user will be charged for. And the monthly fee expected from it
        /// The Key is the Branchid, the double is the monthlyfee for that branch
        /// </summary>
        public Dictionary<string, double> BilledBranchIds { get; set; }

    }
}
