using API.Entities;
using API.Entities.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IFirebaseServices: _IBaseService
    {

        /// <summary>
        /// This is to write in an object an a path, even if it has to overwrite. 
        /// <para> This takes in the <paramref name="thing"/> and the <paramref name="path"/> that it will be stored in </para>
        /// <para> NOTE: This doubles for both storing and editing data in the database cause both overwrite whatever is in the database</para>
        /// </summary>
        /// <returns></returns>
        public void StoreData(string path, object thing);
        /// <summary>
        /// This is to remove anything that is within the <paramref name="fullpath"/> in the database including subfolders
        /// <para>I'm assuming it does it by overwriting anything in there with null</para>
        /// </summary>
        /// <param name="path"></param>
        public void DeleteData(string fullpath);
        /// <summary>
        /// This takes in the path of the node in the database and coughs up the a list of the type
        /// <typeparamref name="T"/>. <typeparamref name="T"/> is the object type you want to have a list of.
        /// <para> NOTE: I didnt put type checking here cause there is data that isn't an entity that could be taken from the database, Like flavour</para>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="path"></param>
        /// <returns> <see cref="List{BaseEntity}"/></returns>
        /// <remarks> Within the method is logic that tries to change the object to a JObject. Note that you cant try for both JArray and JObject</remarks>
        public Task<List<T>> GetData<T>(string path) where T : class, new();
        /// <summary>
        /// This is used when you are collecting aggreagates from the database. It won't work if you are just trying to get a list of Appusers, but it should work if you are trying to get a list of a list of orderItems, ie an Order
        /// <para> In case you still don't understand an aggregate is a list of Entities</para>
        /// </summary>
        /// <typeparam name="Aggregate"></typeparam>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <remarks> Within the method is logic that tries to change the object to a JArray. Note that you cant try for both JArray and JObject</remarks>
        public Task<List<Aggregate>> GetDataArray<Aggregate, Entity>(string path) where Aggregate : BaseAggregates<Entity>, new();

    }
}
