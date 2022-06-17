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
    public class FirebaseServices : _BaseService, IFirebaseServices
    {

        public readonly FirebaseDataContext _firebaseDataContext;
        public FirebaseServices()
        {
            _firebaseDataContext = new FirebaseDataContext();
        }

        public void StoreData(string path, object thing)=> _firebaseDataContext.StoreData(path, thing);
        public void DeleteData(string fullpath) => _firebaseDataContext.DeleteData(fullpath);
        public async Task<List<T>> GetData<T>(string path) where T : class, new()
        {
            List<T> objects = new List<T>();

            var response = await _firebaseDataContext.GetData(path);
            // Here you might get errors cause at some point it was refusing to take the correct overload
            objects = response.FromJsonToObject<T>();

            return objects;
        }
        public async Task<List<T>> GetDataArray<T>(string path) where T : class, new()
        {
            List<T> objects = new List<T>();

            var response = await _firebaseDataContext.GetData(path);
            // Here you might get errors cause at some point it was refusing to take the correct overload
            objects = response.FromJsonToObjectArray<T>();

            return objects;
        }

    }
}
