using API.Data;
using API.Entities;
using API.Extensions;
using API.Interfaces;
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
        // TODO: Add this in every controller so that we have less repeating code
        public readonly FirebaseDataContext _firebaseDataContext;

        public FirebaseServices()
        {
            _firebaseDataContext = new FirebaseDataContext();
        }

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

        public async Task<List<AppUser>> GetAdminAccounts()
        {
            List<AppUser> Users = await GetAllUsers();

            // Removes any one who isn't an admin user
            foreach (var user in Users)
            {
                if (user.Admin == false)
                    Users.Remove(user);
            }

            return Users;
        }
        public async Task<List<BilledUser>> GetBilledAccounts()
        {
            var response = await _firebaseDataContext.GetData("Account/BilledAccounts");
            // REFACTOR: Consider just using the isbilled bool and return those with true. So you don't have to make another node
            return response.FromJsonToObject<BilledUser>();
        }

        //TODO: Add other account firebase calls here please

        #endregion






    }
}
