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

        public DateTime DuePaymentDate { get; set; }
        public float Monthlyfee { get; set; }

        /// <summary>
        /// This is a list of the Branches that the user will be charged for.
        /// </summary>
        public List<string> BilledBranchIds { get; set; }

    }
}
