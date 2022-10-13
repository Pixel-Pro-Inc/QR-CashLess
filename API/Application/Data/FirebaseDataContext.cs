using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Application.Data
{
    public class FirebaseDataContext
    {

        static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        static readonly IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true).Build();

       
        // NOTE: The configuration works properly
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = Configuration["FirebaseDataBaseSettings:AuthSecret"],
            BasePath = Configuration["FirebaseDataBaseSettings:BasePath"]
        };

        IFirebaseClient client;

        public FirebaseDataContext()
        {
            client = new FireSharp.FirebaseClient(config);
        }

        // UPDATE: I change the code cause it kept making new clients, I don't know why it needed to do that
        public async Task DeleteData(string path)=>await client.DeleteAsync(path);
        public async Task StoreData(string path, object data)=>await client.SetAsync(path, data);
        // NOTE: You don't need to refactor this to work with the JsonConvertExtension. But you could clean up with Yewo after discussing it
        public async Task<List<object>> GetData(string path)
        {
            List<object> objects = new List<object>();

            FirebaseResponse response = await client.GetAsync(path);

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

            if (data != null)
            {
                foreach (var item in data)
                {
                    object _object = new object();

                    if (item != null)
                    {
                        // REFACTOR: Try to see if this is the solution we need to make the overload in JsonConvertExtension unified
                        if (item.GetType() == typeof(JProperty))
                        {
                            _object = JsonConvert.DeserializeObject<object>(((JProperty)item).Value.ToString());
                        }
                        else
                        {
                            _object = JsonConvert.DeserializeObject<object>(((JObject)item).ToString());
                        }

                        objects.Add(_object);
                    }
                }
            }

            return objects;
        }
        public async Task EditData(string path, object data)=> await client.UpdateAsync(path, data);

    }
}