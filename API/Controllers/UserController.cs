using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
            
        }


        [HttpGet]
        
        public ActionResult<IEnumerable<AppUser>> GetUser()
        {
            return _context.Users.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<AppUser> GetId(int id)
        {
            return _context.Users.Find(id);
        }

        [HttpPut("{id}")]
        public async Task<bool> UpdateUser(AppUser user)
        {
            var id = await _context.Users.FirstOrDefaultAsync(x=>x.Id== user.Id);

            if(id == null) return false;

            id.UserName = user.UserName;
            id.PasswordHash = user.PasswordHash;
            id.PasswordSalt = user.PasswordSalt;

            return await _context.SaveChangesAsync() > 0;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AppUser>> DeleteUser(int id)
        {
            var Id = await _context.Users.FindAsync(id);
            _context.Users.Remove(Id);
            await _context.SaveChangesAsync();
            return Id;

        }
    }
}