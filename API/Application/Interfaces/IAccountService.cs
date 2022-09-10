using RodizioSmartKernel.Entities;
using RodizioSmartKernel.Interfaces.BaseInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Application.Interfaces
{
    public interface IAccountService: IBaseService
    {
        /// <summary>
        /// Gets the specific user from the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task<AppUser> GetUser(string username);

        /// <summary>
        /// Usually the accountId should be past into this but the conflicting......ah Yewo should explain
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task<AppUser> GetUserByNumber(string phoneNumber);

        /// <summary>
        /// Checks if the username exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns> <see cref="bool"/> true or false</returns>
        public Task<bool> isUserTaken(string username);

        /// <summary>
        /// Creates the user id for the first time. 
        /// <para> We don't expect to use this too much but of course everytime a User is made </para>
        /// </summary>
        /// <returns> an <see cref="int"/> that hasn't been used before</returns>
        public Task<int> CreateId();
        /// <summary>
        /// <para> It basically uses Automapper to map the properties of the AppUsers from the database to the AdminUser</para>
        /// </summary>
        /// <returns> A list of <see cref="AppUser"/>s in type <see cref="AdminUser"/></returns>
        /// <remarks> Returns basically a list of <see cref="AdminUser"/></remarks>
        public Task<List<AdminUser>> GetAdminAccounts();

    }
}
