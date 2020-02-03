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
    public class ProductsController : ControllerBase
    {
        private readonly ChangeContext _context;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(ChangeContext context,ILogger<ProductsController> logger)
        {
            _context = context;//数据库上下文
            _logger = logger;//日志
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            var products = await _context.Product.Select(p => new Product
            {
                ProductId = p.ProductId,
                Title = p.Title,
                MarketPrice = p.MarketPrice,
                Price = p.Price,
                CategoryId = p.CategoryId,
                Photo = p.Photo
            }).ToListAsync();
            return products;
            //return await _context.Product.ToListAsync();
        }
        //public async Task<ActionResult<IEnumerable<IGrouping<int, Product>>>> GetProduct()
        //{
        //    var products = await _context.Product.Select(p => new Product
        //    {
        //        ProductId = p.ProductId,
        //        Title = p.Title,
        //        MarketPrice = p.MarketPrice,
        //        Price = p.Price,
        //        CategoryId = p.CategoryId
        //    }).GroupBy(p => p.CategoryId).ToListAsync();
        //    return products;
        //    //return await _context.Product.ToListAsync();
        //}

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            _logger.LogInformation("调用成功：ProductId=" + id);
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return null;
                //return NotFound();
            }

            return product;

            //var products = await _context.Product.Select(p => new Product
            //{
            //    ProductId = p.ProductId,
            //    CategoryId=p.CategoryId,
            //    Title = p.Title,
            //    MarketPrice = p.MarketPrice,
            //    Price = p.Price,
            //    Photo = p.Photo,
            //    Appraise = p.Appraise
            //}).ToListAsync();

            //if (products == null)
            //{
            //    return null;
            //    //return NotFound();
            //}

            //return products;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
