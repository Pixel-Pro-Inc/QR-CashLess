using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class FirebaseDataContext
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "KIxlMLOIsiqVrQmM0V7pppI1Ao67UPZv5jOdU0QJ",
            BasePath = "https://rodizoapp-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        public async void StoreData(string path, object data)
        {
            client = new FireSharp.FirebaseClient(config);

            var response = await client.SetAsync(path, data);
        }
        public async Task<FirebaseResponse> GetData(string path)
        {
            client = new FireSharp.FirebaseClient(config);

            FirebaseResponse d = await client.GetAsync(path);        

            return d;
        }
        public async void EditData(string path, object data)
        {
            client = new FireSharp.FirebaseClient(config);

            var response = await client.UpdateAsync(path, data);
        }        
    }
}
