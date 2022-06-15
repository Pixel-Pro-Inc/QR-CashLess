using API.Entities;
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
        /// <typeparamref name="T"/> is the object type you want to have a list of
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public Task<List<T>> GetData<T>(string path) where T : class, new();



        /// <summary>
        /// This is left in <see cref="IFirebaseServices"/> cause it has special logic to map AppUsers to Admin
        /// <para> It basically uses Automapper to map the properties of the AppUser to the AdminUser</para>
        /// </summary>
        /// <returns> A list of <see cref="AppUser"/>s in type <see cref="AdminUser"/></returns>
        /// <remarks> Basically a list of <see cref="AdminUser"/></remarks>
        public Task<List<AdminUser>> GetAdminAccounts();

    }
}
