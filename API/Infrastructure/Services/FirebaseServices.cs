using API.Application.Data;
using API.Application.Extensions;
using API.Application.Interfaces;
using RodizioSmartKernel.Core.Entities.Aggregates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Infrastructure.Services
{
    public class FirebaseServices : BaseService, IFirebaseServices
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
            objects = response.FromJsonToObject<T>();

            return objects;
        }
        public async Task<List<Aggregate>> GetDataArray<Aggregate, Entity>(string path) where Aggregate : BaseAggregates<Entity>, new()
        {
            List<Aggregate> objects = new List<Aggregate>();

            var response = await _firebaseDataContext.GetData(path);
            // Here you might get errors cause at some point it was refusing to take the correct overload
            objects = response.FromJsonToObjectArray<Aggregate>();

            return objects;
        }

    }
}
