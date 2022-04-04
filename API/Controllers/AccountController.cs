using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private string dir = "Account";
        private readonly SMSController smsSender;

        public AccountController(ITokenService tokenService)//, UserManager<AppUser> userManager) :base()
        {
            _tokenService = tokenService;
            smsSender = new SMSController();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserTaken(registerDto.Username))
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
                SuperUser = registerDto.SuperUser
            };            

            user.Id = await GetNum();

            _firebaseDataContext.StoreData(dir + "/" + user.Id.ToString(), user);

            return new UserDto()
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)                
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            if ((await GetUser(loginDto.Username)) == null)
                return Unauthorized("Username doesn't exist");

            AppUser user = (await GetUser(loginDto.Username));

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
        
        public async Task<bool> UserTaken(string username)
        {
            List<AppUser> items = await _firebaseDataContext.GetData<AppUser>(dir);

            return items.Any(x => x.UserName == username.ToLower());
        }
        public async Task<AppUser> GetUser(string username)
        {
            List<AppUser> items = await _firebaseDataContext.GetData<AppUser>(dir);

            AppUser user = null;

            user = items.SingleOrDefault(x => x.UserName == username.ToLower());
            return (user);
        }
        public async Task<AppUser> GetUserByNumber(string phoneNumber)
        {
            List<AppUser> items = await _firebaseDataContext.GetData<AppUser>(dir);

            AppUser user = null;

            user = items.SingleOrDefault(x => x.PhoneNumber == phoneNumber);
            return (user);
        }
        public async Task<int> GetNum()
        {
            List<AppUser> items = await _firebaseDataContext.GetData<AppUser>(dir);

            int ran = new Random().Next(10000, 99999);

            while (items.Any(x => x.Id == ran))
            {
                ran = new Random().Next(10000, 99999);
            }

            return ran;
        }

        //When you have set up the Identity roles thing then you can remove the below line to get access to the code

        public async Task<string> GetResetToken()
        {
            string token = "R";

            for (int i = 0; i < 6; i++)
            {
                token += new Random().Next(0, 10);
            }

            return token;
        }

       
        [HttpPost("forgotpassword/{accountID}")]
        public async Task<string> ForgotPassword(string accountID)
        {
            string token = await GetResetToken();

            int result = 0;

            AppUser user = Int32.TryParse(accountID, out result) ? await GetUserByNumber(accountID): user = await GetUser(accountID);

            if (user == null)
                return "failed";

            user.ResetToken = token;

            _firebaseDataContext.EditData("Account/" + user.Id, user);

            //Send SMS
            await smsSender.SendResetPasswordSMS(user.PhoneNumber, token);

            return token;
        }

        [HttpPost("forgotpassword/desktop/{accountID}")]
        public async Task<ActionResult<string>> ForgotPasswordDesktop(string accountID)
        {
            string token = await GetResetToken();

            int result = 0;

            AppUser user = Int32.TryParse(accountID, out result) ? await GetUserByNumber(accountID) : user = await GetUser(accountID);

            if (user == null)
                return "failed";

            user.ResetToken = token;

            _firebaseDataContext.EditData("Account/" + user.Id, user);

            //Send SMS
            await smsSender.SendResetPasswordSMS(user.PhoneNumber, token);

            return token;
        }

        [HttpPost("forgotpassword/successful/{accountID}/{password}")]
        public async Task<string> SetNewPassword(string accountID,string password)
        {
            int result = 0;

            AppUser user = Int32.TryParse(accountID, out result) ? await GetUserByNumber(accountID) : user = await GetUser(accountID);

            user.ResetToken = null;

            using var hmac = new HMACSHA512();

            user.PasswordSalt = hmac.Key;

            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            _firebaseDataContext.EditData("Account/" + user.Id, user);

            return "success";
        }

    }
}
