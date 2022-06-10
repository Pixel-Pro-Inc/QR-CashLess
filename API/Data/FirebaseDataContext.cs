﻿using API.Helpers;
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

       
        // NOTE: The configuration works properly
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = Configuration["FirebaseDataBaseSettings:AuthSecret"],
            BasePath = Configuration["FirebaseDataBaseSettings:BasePath"]
        };

        IFirebaseClient client;

        public async void StoreData(string path, object data)
        {
            client = new FireSharp.FirebaseClient(config);

            var response = await client.SetAsync(path, data);
        }

        // UNDONE: DO NOT SWITCH TO YEWO'S version on Development. The problem with that version is that you have to make a type for everything you want from the database
        // like flavour and shit. We dont need that. Just have this work as is and then the extension will change the type where it is needed
        // @Yewo: Here is my reason. Don't fight and delete unless you consult with me
        // NOTE: You don't need to refactor this to work with the JsonConvertExtension. But you could clean up with Yewo after discussing it
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
