using API.Data;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    /// <summary>
    /// This will be the FirebaseService for all firebase needs
    /// </summary>
    /// <remarks>
    /// Any and all requests  from the database should be found in this partial class
    /// </remarks>

    // UPDATE: This has been changed to work for all since partial classes can use different interfaces and its encapsulated by them
    // REFACTOR: Consider using types here so that we don't have to be specific
    public partial class FirebaseServices : IFirebaseServices
    {

        public readonly FirebaseDataContext _firebaseDataContext;

        public FirebaseServices()
        {
            _firebaseDataContext = new FirebaseDataContext();
        }
        // @Abel: Start with the fix me
        // FIXME: This comes in as null. I think cause this has to be set in the ctor. 
        // This is here so we can make AppUser into AdminUser without losing any data it is also used in AccountController for reference
        static MapperConfiguration mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<AppUser, AdminUser>());
        private Mapper _objectMapper = new Mapper(mapperConfig);

        public void StoreData(string path, object thing)=> _firebaseDataContext.StoreData(path, thing);
        public void DeleteData(string fullpath) => _firebaseDataContext.DeleteData(fullpath);

        #region Order calls

        public async Task<List<List<OrderItem>>> GetOrders(string path, string branchId)
        {
            var response = await _firebaseDataContext.GetData(path + branchId);
            List<List<OrderItem>> items = response.FromJsonToObject<List<OrderItem>>();

            return items;
        }

        //NOTE: This is an overload of the above method
        public async Task<List<OrderItem>> GetOrders(string branchId)
        {
            var response = await _firebaseDataContext.GetData("Order/" + branchId);
            List<OrderItem> items = response.FromJsonToObject<OrderItem>();

            return items;
        }

        #endregion

        #region Menu calls

        public async Task<List<MenuItem>> GetMenu(string branchId)
        {
            var response = await _firebaseDataContext.GetData("Menu/" + branchId);
            List<MenuItem> items = response.FromJsonToObject<MenuItem>();

            return items;
        }

        #endregion

        #region User calls

        public async Task<List<AppUser>> GetAllUsers()
        {
            var response = await _firebaseDataContext.GetData("Account");
            List<AppUser> items = response.FromJsonToObject<AppUser>();
            return items;
        }
        public async Task<AppUser> GetUser(string username)
        {
            List<AppUser> items = await GetAllUsers();

            // This is so we have either a null value returned or the actual user
            AppUser user = null;

            user = items.SingleOrDefault(x => x.UserName == username.ToLower());
            return (user);
        }
        public async Task<bool> isUserTaken(string username)
        {
            List<AppUser> items = await GetAllUsers();
            return items.Any(x => x.UserName == username.ToLower());
        }
        public async Task<int> CreateId()
        {
            List<AppUser> items = await GetAllUsers();

            int ran = new Random().Next(10000, 99999);

            while (items.Any(x => x.Id == ran))
            {
                ran = new Random().Next(10000, 99999);
            }

            return ran;
        }

        public async Task<List<AdminUser>> GetAdminAccounts()
        {
            List<AppUser> Users = await GetAllUsers();
            List<AdminUser> AdminUsers = new List<AdminUser>();
            foreach (var user in Users)
            {
                // TODO: Fix this so that you can turn this properly cause an error is being thrown, then do the same with AccountController line: 61

                var testAdmin = _objectMapper.Map<AppUser, AdminUser>(user); 
                if (user.Admin) AdminUsers.Add(_objectMapper.Map<AppUser, AdminUser>(user));

            }

            return AdminUsers;
        }

        // OBSOLETE: BilledUsers are now simply adminUsers so this is removed
        //public async Task<List<AdminUser>> GetBilledAccounts()
        //{
        //    var response = await _firebaseDataContext.GetData("Account/BilledAccounts");
        //    // REFACTOR: Consider just using the isbilled bool and return those with true. So you don't have to make another node
        //    return response.FromJsonToObject<AdminUser>();
        //}

        //TODO: Add other account firebase calls here please

        #endregion

        #region Branch calls

        public async Task<List<Branch>> GetBranchesFromDatabase()
        {
            var response = await _firebaseDataContext.GetData("Branch");
            List<Branch> branches = response.FromJsonToObject<Branch>();

            return branches;
        }

        #endregion






    }
}
