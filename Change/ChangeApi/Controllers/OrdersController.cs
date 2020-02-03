using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ChangeApi.Models;

namespace ChangeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ChangeContext _context;
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(ChangeContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<Orders>> GetOrders(string OrderId)
        {
            _logger.LogInformation("查询订单,订单编号：" + OrderId);
            return await _context.Orders.FindAsync(OrderId);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Orders>>> GetOrders(int id)
        {
            var orders = await _context.Orders.Where(p => p.UsersId == id).ToListAsync();

            if (orders == null)
            {
                return null;// NotFound();
            }

            return orders;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrders(int id, Orders orders)
        {
            _logger.LogInformation("修改支付状态,订单编号："+ id);
            if (id != orders.OrdersId)
            {
                _logger.LogInformation("修改支付状态失败,订单编号：" + id);
                return BadRequest();
            }

            _context.Entry(orders).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation("修改支付状态成功,订单编号：" + id);
            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Orders>> PostOrders(Orders orders)
        {
            _context.Orders.Add(orders);
            await _context.SaveChangesAsync();

            return orders;
        }

        // DELETE: api/Orders/5
        //[HttpDelete("{id}")]
        [HttpDelete]
        public async Task<ActionResult<Orders>> DeleteOrders(string OrderId)
        {
            var orders = await _context.Orders.FindAsync(OrderId);
            if (orders == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();

            return orders;
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.OrdersId == id);
        }
    }
}
