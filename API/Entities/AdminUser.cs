using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    /// <summary>
    /// This is a User who is expected to be Billed at the end of every month
    /// <para>They are essentially admin, or any one with admin priviledge. 
    /// The Idea is that all admin in a branch will get the invoice and between them they will decide who gets to pay</para>
    /// </summary>
    /// <remarks>
    /// This will either be Managers or whoever is representing the franchansiee
    /// </remarks>
    public class AdminUser:AppUser
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        // TODO: Set this value when the user pays. Which means we need an automatic way of confirming that a payment has come through
        public DateTime LastPaidDate { get; set; }

        // The assumption is the regardless of how many branches you have they we will all be paid at the same time so one date is enough
        public DateTime DuePaymentDate { get; set; }

        [Obsolete]
        /// <summary>
        /// This is a list of the Branches that the user will be charged for and the Current Charge expected from it.
        /// <para> 
        /// The original list should come naturally from <see cref="AppUser.branchId"/></para>
        /// <para>
        ///  <remarks>
        /// The Key is the Branchid, the float is the CurrentCharge for that branch. We use float cause we don't need that much precision for price
        /// </remarks> 
        /// </para>
        /// </summary>
        public Dictionary<string, float> BilledBranchIds { get; set; }

    }
}
