using API.Application.Interfaces;
using AutoMapper;
using RodizioSmartKernel.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Infrastructure.Services
{
    public class AccountService: BaseService, IAccountService
    {
        private readonly IFirebaseServices _IFirebaseServices;
        private readonly IMapper _mapper;

        public AccountService(IFirebaseServices _firebaseServices, IMapper mapper)
        {
            _IFirebaseServices = _firebaseServices;
            _mapper = mapper;
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
        public async Task<List<AdminUser>> GetAdminAccounts()
        {
            List<AppUser> Users = await _IFirebaseServices.GetData<AppUser>("Account");
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
