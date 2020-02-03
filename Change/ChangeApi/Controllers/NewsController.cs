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
    public class NewsController : ControllerBase
    {
        private readonly ChangeContext _context;
        private readonly ILogger<NewsController> _logger;
        public NewsController(ChangeContext context, ILogger<NewsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews(string NTypes)
        {
            _logger.LogInformation("查询活动调用成功：NTypes=" + NTypes);
            var result = await _context.News.Where(u => u.Ntypes== NTypes).ToListAsync();
            if (result.Count == 0)
            {
                _logger.LogInformation("未查到记录...");
                return null;
                //return NotFound();
            }

            return result;
        }


        // GET: api/News/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<News>> GetNews(int id)
        //{
        //    var news = await _context.News.FindAsync(id);

        //    if (news == null)
        //    {
        //        return NotFound();
        //    }

        //    return news;
        //}

        // PUT: api/News/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews(int id, News news)
        {
            if (id != news.NewsId)
            {
                return BadRequest();
            }

            _context.Entry(news).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
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

        // POST: api/News
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<News>> PostNews(News news)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNews", new { id = news.NewsId }, news);
        }

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<News>> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return news;
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.NewsId == id);
        }
    }
}
