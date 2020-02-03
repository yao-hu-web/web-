using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChangeHomeMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace WebBackstage.Controllers
{
    //[Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly ChangeContext _context;
        private readonly ILogger<LoginController> _logger;
        public LoginController(ChangeContext context,ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(AdminUser users)
        {
            _logger.LogInformation("查询管理员：UserName=" + users.UserName);
            var recvUser = await _context.AdminUser.Where(p => p.UserName == users.UserName).ToListAsync();
            if (recvUser != null)
            {                
                string pagePwd = users.Pwd;
                if (recvUser[0].UserName == users.UserName && recvUser[0].Pwd.ToLower() == pagePwd.ToLower())
                {
                    HttpContext.Session.SetString("AdminUser", recvUser[0].UserName);//缓存当前登录用户
                    HttpContext.Session.SetString("AdminUserId", recvUser[0].AdminUserId.ToString());//缓存当前登录用户Id
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
            //return View(await _context.AdminUser.ToListAsync());
        }
        [HttpPost]        
        public async Task<ActionResult> Login([FromBody]AdminUser users)
        {
            _logger.LogInformation("查询管理员：UserName=" + users.UserName);
            var recvUser = await _context.AdminUser.Where(p => p.UserName == users.UserName).ToListAsync();
            if (recvUser != null)
            {
                if (recvUser[0].UserName == users.UserName && recvUser[0].Pwd.ToLower() == users.Pwd.ToLower())
                {
                    HttpContext.Session.SetString("AdminUser", recvUser[0].UserName);//缓存当前登录用户
                    HttpContext.Session.SetString("AdminUserId", recvUser[0].AdminUserId.ToString());//缓存当前登录用户Id
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Index", "Login");

        }

        // GET: Login/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUser
                .FirstOrDefaultAsync(m => m.AdminUserId == id);
            if (adminUser == null)
            {
                return NotFound();
            }

            return View(adminUser);
        }

        // GET: Login/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminUsersId,UserName,Pwd,Role")] AdminUser adminUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminUser);
        }

        // GET: Login/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUser.FindAsync(id);
            if (adminUser == null)
            {
                return NotFound();
            }
            return View(adminUser);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminUsersId,UserName,Pwd,Role")] AdminUser adminUser)
        {
            if (id != adminUser.AdminUserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminUserExists(adminUser.AdminUserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(adminUser);
        }

        // GET: Login/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminUser = await _context.AdminUser
                .FirstOrDefaultAsync(m => m.AdminUserId == id);
            if (adminUser == null)
            {
                return NotFound();
            }

            return View(adminUser);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminUser = await _context.AdminUser.FindAsync(id);
            _context.AdminUser.Remove(adminUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminUserExists(int id)
        {
            return _context.AdminUser.Any(e => e.AdminUserId == id);
        }
    }
}
