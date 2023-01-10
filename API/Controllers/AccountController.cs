using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        
        private readonly DataContext _context;
        private readonly ITokenService _TokenService;
        public AccountController(DataContext context, ITokenService TokenService)
        {
            _TokenService = TokenService;
            _context = context;
        }

    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> Register(RegisterDto register)
    {
        if(await UserExists(register.Username)) return BadRequest("Username is taken");
        using var hmac= new HMACSHA512();

        var user = new AppUser
        {
            UserName = register.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
            PasswordSalt = hmac.Key
        };
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
        // return new UserDto{
        //     Username = user.UserName,
        //     Token = _TokenService.CreateToken(user)
        // };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> login(LogInDto LogIn)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x=>x.UserName == LogIn.Username);
        if(user == null) return Unauthorized("Invalid Username");

       using var hmac = new HMACSHA512(user.PasswordSalt);
        var passhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(LogIn.Password));

        for(int i=0; i<passhash.Length; i++)
        {
            if(passhash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }
        //return user;
        return new UserDto{
            Username= user.UserName.ToLower(),
            Token = _TokenService.CreateToken(user)
        };
    }
    private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        
    }
}