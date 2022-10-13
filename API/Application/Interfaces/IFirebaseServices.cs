using RodizioSmartKernel.Core.Entities.Aggregates;
using RodizioSmartKernel.Application.Interfaces.BaseInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using RodizioSmartKernel.Application.Interfaces;

namespace API.Application.Interfaces
{
    // REFACTOR: Consider having a IDataBaseService that IFirebase inherits from, that way we can switch database providers when ever we feel like.
    /// <summary>
    /// This will be the FirebaseService for all firebase needs
    /// </summary>
    /// <remarks>
    /// Any and all requests  from the Firebasedatabase should be found in this partial class
    /// </remarks>
    public interface IFirebaseServices: IBaseService, IDataService
    {

        /// <summary>
        /// This is to write in an object an a path, even if it has to overwrite. 
        /// <para> This takes in the <paramref name="thing"/> and the <paramref name="path"/> that it will be stored in </para>
        /// <para> NOTE: This doubles for both storing and editing data in the database cause both overwrite whatever is in the database</para>
        /// </summary>
        /// <returns></returns>
        public Task StoreData(string path, object thing);

        /// <summary>
        /// This is to remove anything that is within the <paramref name="fullpath"/> in the database including subfolders
        /// <para>I'm assuming it does it by overwriting anything in there with null</para>
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public Task DeleteData(string fullpath);
        // FIXME: We need to remove the clases that don't inherit from baseEntity. If any object can pass through here, then aggregates can mistakenly be put here
        /// <summary>
        /// This takes in the path of the node in the database and coughs up the a list of the type
        /// <typeparamref name="T"/>. <typeparamref name="T"/> is the object type you want to have a list of.
        /// <para> NOTE: I didnt put type checking here cause there is data that doesn't inherit from <see cref="BaseEntity"/> that could be taken from the database, Like <see cref="Flavour"/>.
        /// This means that an error can be thrown if you try to pass in an aggregate. See <see cref="FailedToConvertFromJson"/> for more info.</para> 
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="path"></param>
        /// <returns> <see cref="List{BaseEntity}"/></returns>
        /// <remarks> Within the method is logic that tries to change the response to a JObject. Note that you cant try the response for both JArray and JObject</remarks>
        public Task<List<T>> GetData<T>(string path) where T : class, new();
        /// <summary>
        /// This is used when you are collecting aggreagates from the database. For example, it won't work if you are just trying to get a list of <see cref="AppUser"/>s, but it should work if you are trying to get a list of a list of orderItems, ie an <see cref="Order"/>
        /// <para> In case you still don't understand an aggregate is a list of Entities. Check <see cref="BaseAggregates{T}"/> for more clues. If you find yourself forced to get a <see cref="JArray"/> response ( searching for a list of a list of anything), you should make an Aggregate type
        /// of the item you are searching for. So that it can 'contain' it</para>
        /// </summary>
        /// <typeparam name="Aggregate"></typeparam>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <remarks> Within the method is logic that tries to change the response to a JArray. Note that you cant try the response for both JArray and JObject</remarks>
        public Task<List<Aggregate>> GetDataArray<Aggregate, Entity>(string path) where Aggregate : BaseAggregates<Entity>, new();

    }
}
