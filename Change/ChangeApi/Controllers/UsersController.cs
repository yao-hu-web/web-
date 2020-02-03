using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChangeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace ChangeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ChangeContext _context;
        private readonly ILogger<UsersController> _logger;
        public UsersController(ChangeContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Users
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        //{
        //    return await _context.Users.ToListAsync();
        //}

        // GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Users>> GetUsers(int id)
        //{
        //    var users = await _context.Users.FindAsync(id);

        //    if (users == null)
        //    {
        //        return NotFound();
        //    }

        //    return users;
        //}
        [HttpGet]
        public async Task<ActionResult<Users>> GetUsers(string UserName)
        {
            _logger.LogInformation("查询用户调用成功：UserName=" + UserName);
            var users = await _context.Users.Where(u => u.UserName.Contains(UserName)).ToListAsync();
            if (users.Count == 0)
            {
                _logger.LogInformation("未查到记录...");
                return null;
                //return NotFound();
            }

            return users[0];
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.UsersId)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        //public async Task<ActionResult<Users>> PostUsers()
        //{
        //    _logger.LogInformation("注册调用成功");
        //    return null;
        //}
        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            _logger.LogInformation("注册调用成功：UserName=" + users.UserName);
            var result = await _context.Users.Where(u => u.UserName.Contains(users.UserName)).ToListAsync();
            if (result.Count == 0)
            {
                _context.Users.Add(users);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUsers", new { id = users.UsersId }, users);
            }
            else
            {
                return null;
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> DeleteUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return users;
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UsersId == id);
        }
    }
}
