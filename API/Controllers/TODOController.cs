using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class TODOController : BaseApiController
    {
        private readonly DataContext _context;
        public TODOController(DataContext context)
        {
            _context = context;
            
        }

        [HttpGet]
        public ActionResult<IEnumerable<ObjUser>> GetUser()
        {
            return _context.Obj.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<ObjUser> GetId(int id)
        {
            var Num = _context.Obj.Find(id);

            if(Num == null)
            {
                return NotFound();
            }
            return Ok(Num);
        }

        [HttpPost("insert")]
        public async Task<ActionResult<ObjUser>> AddData(string task)
        {
            if(await UserExists(task)) return BadRequest("Task Existed");

            var user = new ObjUser
            {
               // Id = id,
                Status = task
            };
            _context.Obj.Add(user);
            await _context.SaveChangesAsync();
            return user;
            
        }

        [HttpPut("{id}")]
        public async Task<bool> UpdateUser(ObjUser user)
        {
            var id = await _context.Obj.FirstOrDefaultAsync(x=>x.Id== user.Id);

            if(id == null) return false;

            id.Status = user.Status;

            return await _context.SaveChangesAsync() > 0;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ObjUser>> DeleteUser(int id)
        {
            var Id = await _context.Obj.FindAsync(id);
            _context.Obj.Remove(Id);
            await _context.SaveChangesAsync();
            return Id;

        }
        private async Task<bool> UserExists(string Status)
        {
            return await _context.Obj.AnyAsync(x => x.Status == Status.ToLower());
        }

    }
}