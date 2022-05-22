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
    /// This will be the FirebaseService for accounts only.
    /// <remarks>
    /// Any and all requests about accounts and users from the database should be found in this partial class
    /// </remarks>
    /// </summary>

    // REFACTOR: Consider using types here so that we don't have to be specific
    public partial class FirebaseServices : IFirebaseServices
    {
        // REFACTOR: Since this is the only place where we expect to do the deserialization, it is here where we will make sure the extention works for the JsonConverter
        public readonly FirebaseDataContext _firebaseDataContext;

        public FirebaseServices()
        {
            _firebaseDataContext = new FirebaseDataContext();
        }

        public async Task<AppUser> GetUser(string username) 
        {
            var response = await _firebaseDataContext.GetData("Account");
            List<AppUser> items = new List<AppUser>();
            for (int i = 0; i < response.Count; i++)
            {
                var item = response[i];

                AppUser data = JsonConvert.DeserializeObject<AppUser>(((JObject)item).ToString());

                items.Add(data);
            }

            AppUser user = null;

            user = items.SingleOrDefault(x => x.UserName == username.ToLower());
            return (user);
        }

        public async Task<List<BilledUser>> GetBilledAccounts()
        {
            var response = await _firebaseDataContext.GetData("Account/BilledAccounts");
            List<BilledUser> items = new List<BilledUser>();
            for (int i = 0; i < response.Count; i++)
            {
                BilledUser item = JsonConvert.DeserializeObject<BilledUser>(((JObject)response[i]).ToString());

                items.Add(item);
            }
            return items;
        }


    }
}
