using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChangeApi.Models;

namespace ChangeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppraisesController : ControllerBase
    {
        private readonly ChangeContext _context;

        public AppraisesController(ChangeContext context)
        {
            _context = context;
        }

        // GET: api/Appraises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appraise>>> GetAppraise()
        {
            return await _context.Appraise.ToListAsync();
        }

        // GET: api/Appraises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Appraise>>> GetAppraise(int id)
        {
            var appraise = await _context.Appraise.Where(p => p.UsersId == id).ToListAsync();

            if (appraise == null)
            {
                return null;// NotFound();
            }

            return appraise;
        }

        // PUT: api/Appraises/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppraise(int id, Appraise appraise)
        {
            if (id != appraise.AppraiseId)
            {
                return BadRequest();
            }

            _context.Entry(appraise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppraiseExists(id))
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

        // POST: api/Appraises
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Appraise>> PostAppraise(Appraise appraise)
        {
            _context.Appraise.Add(appraise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppraise", new { id = appraise.AppraiseId }, appraise);
        }

        // DELETE: api/Appraises/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Appraise>> DeleteAppraise(int id)
        {
            var appraise = await _context.Appraise.FindAsync(id);
            if (appraise == null)
            {
                return NotFound();
            }

            _context.Appraise.Remove(appraise);
            await _context.SaveChangesAsync();

            return appraise;
        }

        private bool AppraiseExists(int id)
        {
            return _context.Appraise.Any(e => e.AppraiseId == id);
        }
    }
}
