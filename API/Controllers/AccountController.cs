using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public AccountController(ITokenService tokenService, IFirebaseServices firebaseServices) :base(firebaseServices)
        {
            _tokenService = tokenService;
            // UPDATE: I removed all references to firebaseDatacontext and replaces it with _firebaseServices
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _firebaseServices.isUserTaken(registerDto.Username))
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

            // TODO: add logic for if that person is a billable user
            // which means the registerDto has to include isBillable
            // _firebaseDataContext.StoreData("Account/BilledAccounts" + "/" + user.Id.ToString(), user);


            user.Id = await _firebaseServices.CreateId();

            _firebaseServices.StoreData(dir + "/" + user.Id.ToString(), user);

            return new UserDto()
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)                
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            if ((await _firebaseServices.GetUser(loginDto.Username)) == null)
                return Unauthorized("Username doesn't exist");

            AppUser user = (await _firebaseServices.GetUser(loginDto.Username));

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

        [HttpGet("getadminusers")]
        public async Task<List<AdminUser>> GetAdminUsers() => await _firebaseServices.GetAdminAccounts();

    }
}
