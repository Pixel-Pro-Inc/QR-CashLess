using API.Data;
using API.Entities;
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
        // TODO: Since this is the only place where we expect to do the deserialization, it is here where we will make sure the extention works for the JsonConverter
        public readonly FirebaseDataContext _firebaseDataContext;

        public FirebaseServices()
        {
            _firebaseDataContext = new FirebaseDataContext();
        }

        public async Task<AppUser> GetUser(string username) 
        {
            var response = await _firebaseDataContext.GetData("Account");
            List<AppUser> items = getResponseList<AppUser>(response);

            // This is so we have either a null value returned or the actual user
            AppUser user = null;

            user = items.SingleOrDefault(x => x.UserName == username.ToLower());
            return (user);
        }
        public async Task<List<AppUser>> GetAdminAccounts()
        {
            var response = await _firebaseDataContext.GetData("Account");
            List<AppUser> Users = getResponseList<AppUser>(response);

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
            return getResponseList<BilledUser>(response);
        }
       

        //TODO: Add other account firebase calls here please

        // REFACTOR: This should be in the JsonConverterExtension
       /// <summary>
       ///  Gives the response from the firebase context in the expected format
       /// </summary>
       /// <typeparam name="T"> This should be any thing that the base entity is a parent of</typeparam>
       /// <param name="response"> This respons is to come from the firebaseDatacontext get method</param>
       /// <returns>List of type <typeparamref name="T"/></returns>
        List<T> getResponseList<T>(List<object> response)where T : BaseEntity
        {
            List<T> items = new List<T>();
            for (int i = 0; i < response.Count; i++)
            {
                // This adds the deserialized list in the format into the type we are returning
                items.Add(JsonConvert.DeserializeObject<T>(((JObject)response[i]).ToString()));
            }
            return items;

        }


    }
}
