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

        /// <summary>
        /// Returns all accounts that are adminstrator level
        /// </summary>
        /// <returns> A list of administator users in type <see cref="AppUser"/></returns>
        public Task<List<AppUser>> GetAdminAccounts();

        /// <summary>
        /// Gets all the users under the firebase directory 'Account'
        /// </summary>
        /// <returns> List of <see cref="AppUser"/></returns>
        public Task<List<AppUser>> GetAllUsers();

        /// <summary>
        /// Gets the specific user from the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task<AppUser> GetUser(string username);

        /// <summary>
        /// Checks if the username exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns> <see cref="bool"/> true or false</returns>
        public Task<bool> isUserTaken(string username);

        /// <summary>
        /// Creates the user id for the first time. 
        /// <para> We don't expect to use this too much but of course everytime a User is made </para>
        /// </summary>
        /// <returns> an <see cref="int"/> that hasn't been used before</returns>
        public Task<int> CreateId();

        /// <summary>
        /// Gets the branches in the database under the node 'Branch'
        /// 
        /// <para>
        /// The list of objects is Converted with <see cref="JsonConvertExtensions"/> into the return type</para>
        /// </summary>
        /// <returns> List of <see cref="Branch"/></returns>
        public Task<List<Branch>> GetBranchesFromDatabase();
    }
}
