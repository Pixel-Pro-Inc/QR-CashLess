using API.Helpers;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class FirebaseDataContext
    {
        static readonly IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = Configuration["Firebase:AuthSecret"],
            BasePath = Configuration["Firebase:BasePath"]
        };

        IFirebaseClient client;

        public async void StoreData(string path, object data)
        {
            client = new FireSharp.FirebaseClient(config);

            var response = await client.SetAsync(path, data);
        }
        public async Task<List<object>> GetData(string path)
        {
            List<object> objects = new List<object>();

            client = new FireSharp.FirebaseClient(config);

            FirebaseResponse response = await client.GetAsync(path);

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

            if (data != null)
            {
                foreach (var item in data)
                {
                    object _object = new object();

                    if(item != null)
                    {
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
        public async void EditData(string path, object data)
        {
            client = new FireSharp.FirebaseClient(config);

            var response = await client.UpdateAsync(path, data);
        }        
        public async void DeleteData(string fullPath)
        {
            client = new FireSharp.FirebaseClient(config);

            await client.DeleteAsync(fullPath);
        }
    }
}
