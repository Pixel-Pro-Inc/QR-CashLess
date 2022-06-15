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
        private readonly IMapper _mapper;

        public FirebaseServices(IMapper mapper)
        {
            _firebaseDataContext = new FirebaseDataContext();
            _mapper = mapper;
        }

        public void StoreData(string path, object thing)=> _firebaseDataContext.StoreData(path, thing);
        public void DeleteData(string fullpath) => _firebaseDataContext.DeleteData(fullpath);
        public async Task<List<T>> GetData<T>(string path) where T : class, new()
        {
            List<T> objects = new List<T>();

            var response = await _firebaseDataContext.GetData(path);

            objects = response.FromJsonToObject<T>();

            return objects;
        }

        public async Task<List<AdminUser>> GetAdminAccounts()
        {
            List<AppUser> Users = await GetData<AppUser>("Account");
            List<AdminUser> AdminUsers = new List<AdminUser>();
            foreach (var user in Users)
            {
                var testAdmin = _mapper.Map<AdminUser>(user);
                if (testAdmin.Admin) AdminUsers.Add(testAdmin);

            }

            return AdminUsers;
        }

    }
}
