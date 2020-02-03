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
    public class OrdersDetailsController : ControllerBase
    {
        private readonly ChangeContext _context;

        public OrdersDetailsController(ChangeContext context)
        {
            _context = context;
        }

        // GET: api/OrdersDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdersDetail>>> GetOrdersDetail()
        {
            return await _context.OrdersDetail.ToListAsync();
        }

        // GET: api/OrdersDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdersDetail>> GetOrdersDetail(int id)
        {
            var ordersDetail = await _context.OrdersDetail.FindAsync(id);

            if (ordersDetail == null)
            {
                return NotFound();
            }

            return ordersDetail;
        }

        // PUT: api/OrdersDetails/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrdersDetail(int id, OrdersDetail ordersDetail)
        {
            if (id != ordersDetail.OrdersDetailId)
            {
                return BadRequest();
            }

            _context.Entry(ordersDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersDetailExists(id))
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

        // POST: api/OrdersDetails
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<OrdersDetail>> PostOrdersDetail(OrdersDetail ordersDetail)
        {
            _context.OrdersDetail.Add(ordersDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrdersDetail", new { id = ordersDetail.OrdersDetailId }, ordersDetail);
        }

        // DELETE: api/OrdersDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrdersDetail>> DeleteOrdersDetail(int id)
        {
            var ordersDetail = await _context.OrdersDetail.FindAsync(id);
            if (ordersDetail == null)
            {
                return NotFound();
            }

            _context.OrdersDetail.Remove(ordersDetail);
            await _context.SaveChangesAsync();

            return ordersDetail;
        }

        private bool OrdersDetailExists(int id)
        {
            return _context.OrdersDetail.Any(e => e.OrdersDetailId == id);
        }
    }
}
