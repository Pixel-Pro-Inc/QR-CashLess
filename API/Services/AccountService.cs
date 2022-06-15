using API.Entities;
using API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class AccountService: IAccountService
    {
        private readonly IFirebaseServices _IFirebaseServices;

        public AccountService(IFirebaseServices _firebaseServices)
        {
            _IFirebaseServices = _firebaseServices;
        }

        // REFACTOR: Check if you can ToLower numbers, if it doesnt throw and error, fuse the below two methods
        public async Task<AppUser> GetUser(string username)
        {
            List<AppUser> items = await _IFirebaseServices.GetData<AppUser>("Account");

            // This is so we have either a null value returned or the actual user
            AppUser user = null;

            user = items.SingleOrDefault(x => x.UserName == username.ToLower());
            return (user);
        }
        public async Task<AppUser> GetUserByNumber(string phoneNumber)
        {
            List<AppUser> items = await _IFirebaseServices.GetData<AppUser>("Account");

            AppUser user = null;

            user = items.SingleOrDefault(x => x.PhoneNumber == phoneNumber);
            return (user);
        }
        public async Task<bool> isUserTaken(string username)
        {
            List<AppUser> items = await _IFirebaseServices.GetData<AppUser>("Account");
            return items.Any(x => x.UserName == username.ToLower());
        }
        public async Task<int> CreateId()
        {
            List<AppUser> items = await _IFirebaseServices.GetData<AppUser>("Account");

            int ran = new Random().Next(10000, 99999);

            while (items.Any(x => x.Id == ran))
            {
                ran = new Random().Next(10000, 99999);
            }

            return ran;
        }

    }
}
