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
        /// This is to write in an object an a path, even if it has to overwrite. 
        /// <para> This takes in the <paramref name="thing"/> and the <paramref name="path"/> that it will be stored in </para>
        /// <para> NOTE: This doubles for both storing and editing data in the database cause both overwrite whatever is in the database</para>
        /// </summary>
        /// <returns></returns>
        public void StoreData(string path, object thing);

        /// <summary>
        /// This is to remove anything that is within the <paramref name="fullpath"/> in the database including subfolders
        /// <para>I'm assuming it does it by overwriting anything in there with null</para>
        /// </summary>
        /// <param name="path"></param>
        public void DeleteData(string fullpath);


        // OBSOLETE: BilledUsers are now simply adminUsers so this is removed
        /// <summary>
        /// This gets all accounts that are adminstrator level. This also mean everyone who is sent the billing invoice
        /// </summary>
        /// <remarks>
        /// It also seems too much to have a method to get the billed branches from the branch node,
        /// cause that would require us to change the branch entity (again).
        /// <para> So as it stands it gets them from "Account/AdminAccounts"</para>
        /// </remarks>
        /// <returns>A list of administator users in type <see cref="AdminUser"/></returns>
        //public Task<List<AdminUser>> GetBilledAccounts();

        /// <summary>
        /// Returns 
        /// </summary>
        /// <returns> A list of administator users in type <see cref="AppUser"/></returns>
        public Task<List<AdminUser>> GetAdminAccounts();

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
        /// Usually the accountId should be past into this but the conflicting......ah Yewo should explain
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task<AppUser> GetUserByNumber(string phoneNumber);

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

        /// <summary>
        /// This takes one of three paths ( "CompletedOrders", "CancelledOrders", "UnCompletedOrders") and the branchId
        /// <para> then it combines them to make the node path to get the orders within that node</para>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="branchId"></param>
        /// <returns> List of 'Orders' as a List of a List of <see cref="OrderItem"/></returns>
        public Task<List<List<OrderItem>>> GetOrders(string path , string branchId);

        /// <summary>
        /// An overload of <see cref="GetOrders(string, string)"/>
        /// <para> This one just takes the branch Id and returns a single list of orders in that "Order/"<paramref name="branchId"/></para>
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns> 'Orders' as a List of <see cref="OrderItem"/></returns>
        public Task<List<OrderItem>> GetOrders(string branchId);

        /// <summary>
        /// This takes the branch Id and gets the menu items that are available in that branch 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="branchId"></param>
        /// <returns> List of <see cref="MenuItem"/></returns>
        public Task<List<MenuItem>> GetMenu(string branchId);

        /// <summary>
        /// This is to get all the SMS in the branch that has been sent
        /// </summary>
        /// <returns></returns>
        public Task<List<SMS>> GetSMSinBranch();

        /// <summary>
        /// This is Yewo's refactoring that would take in any generic class and the <paramref name="path"/> and then get the data
        /// I fuse it in with my own logic so that it uses the JsonConvertExtension.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public Task<List<T>> GetData<T>(string path) where T : class, new();

    }
}
