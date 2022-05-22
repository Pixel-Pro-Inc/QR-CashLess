using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IFirebaseServices
    {
        // TODO: Have all the firebase methods that are called throughout the project defined here
        // Even though there will be many partial classes, I think it would be wiser to put all the methods here cause there will
        // be alot of methods but it would be good to track them all in regions representing each partial class

        /// <summary>
        /// Gets the specific user from the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task<AppUser> GetUser(string username);

        /// <summary>
        /// This gets all the Billed Users from the database.
        /// </summary>
        /// <remarks>
        /// It also seems too much to have a method to get the billed branches from the branch node,
        /// cause that would require us to change the brannch entity (again).
        /// <para> So as it stands it gets them from "Account/BilledAccounts"</para>
        /// </remarks>
        /// <returns></returns>
        public Task<List<BilledUser>> GetBilledAccounts();

    }
}
