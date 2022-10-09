using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RodizioSmartKernel.Core.Entities;
using API.Application.Interfaces;
using API.Application.DTOs;

namespace API.Infrastructure.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly SMSController smsSender;


        public AccountController(ITokenService tokenService, IFirebaseServices firebaseServices, IAccountService accountService, IMapper mapper) :base(firebaseServices)
        {
            _tokenService = tokenService;
            _accountService = accountService;
            _mapper = mapper;
            // UPDATE: I removed all references to firebaseDatacontext and replaces it with _firebaseServices
            smsSender = new SMSController(_firebaseServices);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _accountService.isUserTaken(registerDto.Username))
                return BadRequest("Username is not available.");

            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            using var hmac = new HMACSHA512();
            var user = new AppUser()
            {
                UserName = registerDto.Username.ToLower(),
                FirstName = myTI.ToTitleCase(registerDto.Firstname.ToLower()),
                LastName = myTI.ToTitleCase(registerDto.Lastname.ToLower()),
                PhoneNumber = registerDto.Phonenumber.ToString(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                Developer = registerDto.Developer,
                branchId = registerDto.branchId,
                Admin = registerDto.Admin,
                SuperUser = registerDto.SuperUser,
                Email = registerDto.Email,
                NationalIdentityNumber = registerDto.NationalIdentityNumber
            };            

            user.Id = await _accountService.CreateId();

            // Here I want to set the user to an AdminUser with the appropriate properties if admin=true
            if (user.Admin)
            {

                AdminUser adminuser= _mapper.Map<AdminUser>(user);

                //FIXME: Add these properties to the view
                //Setting of the AdminUser properties
                adminuser.Fullname = registerDto.Fullname;
                adminuser.Email = registerDto.Email;
                adminuser.Address = registerDto.Address;
                // REFACTOR: Consider having this set to a specific period of time like, 8:00 in the morning
                adminuser.DuePaymentDate = registerDto.DuePaymentDate;

                // This is so the user is stored as an AdminUser with the properties it needs
                user = adminuser;
            }

            _firebaseServices.StoreData("Account" + "/" + user.Id.ToString(), user);

            return new UserDto()
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)                
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            AppUser user = (await _accountService.GetUser(loginDto.Username));

            if (user == null)
                return Unauthorized("Username doesn't exist");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            Byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Wrong password");
            }

            //Successful
            return new UserDto()
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                Developer = user.Developer,
                branchId = user.branchId,
                Admin = user.Admin,
                SuperUser = user.SuperUser
            };
        }
        

        //When you have set up the Identity roles thing then you can remove the below line to get access to the code

        public string GetResetToken()
        {
            string token = "R";

            for (int i = 0; i < 6; i++)
            {
                token += new Random().Next(0, 10);
            }

            return token;
        }

        // REFACTOR: Below we have repeating code
        [HttpPost("forgotpassword/{accountID}")]
        public async Task<string> ForgotPassword(string accountID)
        {
            string token =  GetResetToken();

            int result = 0;

            AppUser user = Int32.TryParse(accountID, out result) ? await _accountService.GetUserByNumber(accountID): user = await _accountService.GetUser(accountID);

            if (user == null)
                return "failed";

            user.ResetToken = token;

            _firebaseServices.StoreData("Account/" + user.Id, user);

            //Send SMS
            await smsSender.SendResetPasswordSMS(user.PhoneNumber, token);

            return token;
        }

        [HttpPost("forgotpassword/desktop/{accountID}")]
        public async Task<ActionResult<string>> ForgotPasswordDesktop(string accountID)
        {
            string token = GetResetToken();

            int result = 0;

            AppUser user = Int32.TryParse(accountID, out result) ? await _accountService.GetUserByNumber(accountID) : user = await _accountService.GetUser(accountID);

            if (user == null)
                return "failed";

            user.ResetToken = token;

            _firebaseServices.StoreData("Account/" + user.Id, user);

            //Send SMS
            await smsSender.SendResetPasswordSMS(user.PhoneNumber, token);

            return token;
        }

        [HttpPost("forgotpassword/successful/{accountID}/{password}")]
        public async Task<string> SetNewPassword(string accountID,string password)
        {
            int result = 0;

            AppUser user = Int32.TryParse(accountID, out result) ? await _accountService.GetUserByNumber(accountID) : await _accountService.GetUser(accountID);

            user.ResetToken = null;

            using var hmac = new HMACSHA512();

            user.PasswordSalt = hmac.Key;

            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            _firebaseServices.StoreData("Account/" + user.Id, user);

            return "success";
        }
        [HttpGet("getadminusers")]
        public async Task<List<AdminUser>> GetAdminUsers() => await _accountService.GetAdminAccounts();

    }
}
