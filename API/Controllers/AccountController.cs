using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly FirebaseDataContext _firebaseDataContext;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _firebaseDataContext = new FirebaseDataContext();
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, string branchId)
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
                branchId = branchId,
                Admin = registerDto.Admin
            };

            //_firebaseDataContext.StoreData("Account/" + branchId + "/" + id, user);

            return new UserDto()
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)                
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            if (!(await UserTaken(loginDto.Username)))
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
                Admin = user.Admin
            };
        }

        public async Task<bool> UserTaken(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        public async Task<AppUser> GetUser(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username.ToLower());
            return (AppUser)(user);
        }
    }
}
