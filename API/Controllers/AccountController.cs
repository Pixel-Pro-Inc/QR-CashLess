using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AutoMapper;

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
        private readonly IMapper _mapper;


        public AccountController(ITokenService tokenService, IFirebaseServices firebaseServices, IMapper mapper) :base(firebaseServices)
        {
            _tokenService = tokenService;
            _mapper = mapper;
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

            user.Id = await _firebaseServices.CreateId();

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
