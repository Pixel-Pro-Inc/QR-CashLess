using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private string dir = "Account";
        private readonly IEmailSender _emailSender;
        private readonly SMSController smsSender; //I added this here cause I did'nt know any other way to involve the method it contains. we use it in line 159

        //private readonly IdentityManager _userManager; // This here should have it's type replaced with whatever you make for ID role management

        public AccountController(ITokenService tokenService, IEmailSender emailSender/*, IdentityManager userManager*/) :base()
        {
            _tokenService = tokenService;
            _emailSender = emailSender;
            //_userManager= userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserTaken(registerDto.Username))
                return BadRequest("Username is not available.");

            using var hmac = new HMACSHA512();
            var user = new AppUser()
            {
                UserName = registerDto.Username.ToLower(),
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
            var response = await _firebaseDataContext.GetData(dir);
            List<AppUser> items = new List<AppUser>();
            for (int i = 0; i < response.Count; i++)
            {
                AppUser item = JsonConvert.DeserializeObject<AppUser>(((JObject)response[i]).ToString());

                items.Add(item);
            }

            return items.Any(x => x.UserName == username.ToLower());
        }
        public async Task<AppUser> GetUser(string username)
        {
            var response = await _firebaseDataContext.GetData(dir);
            List<AppUser> items = new List<AppUser>();
            for (int i = 0; i < response.Count; i++)
            {
                var item = response[i];

                AppUser data = JsonConvert.DeserializeObject<AppUser>(((JObject)item).ToString());

                items.Add(data);
            }

            AppUser user = null;

            user = items.SingleOrDefault(x => x.UserName == username.ToLower());
            return (user);
        }
        public async Task<int> GetNum()
        {
            var response = await _firebaseDataContext.GetData(dir);
            List<AppUser> items = new List<AppUser>();
            for (int i = 0; i < response.Count; i++)
            {
                AppUser item = JsonConvert.DeserializeObject<AppUser>(((JObject)response[i]).ToString());

                items.Add(item);
            }

            int ran = new Random().Next(10000, 99999);

            while (items.Any(x => x.Id == ran))
            {
                ran = new Random().Next(10000, 99999);
            }

            return ran;
        }

        //When you have set up the Identity roles thing then you can remove the below line to get access to the code
#if IdentityroleManagerisNotReady==true
       
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email); //We will have to get these from our Identity role management logic
            if (user == null)
                return BadRequest("Invalid Request");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                 {"token", token },
                 {"email", forgotPasswordDto.Email }
            };
            await smsSender.SendResetPasswordSMS(user.number, token);
            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);

            var message = new Message(new string[] { user.Email }, "Reset password token", callback);
            await _emailSender.SendEmailAsync(message); //I don't expect this to work cause an SmptServer account hasn't been created. Im tempted just to comment anything that 
            // has to do with email in this method.

            return Ok();
        }
#endif

    }
}
