using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using ChangeMvc.Models;

namespace ChangeMvc.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(Users users)
        {
            HttpClient client = new HttpClient();
            //var result = await client.GetStringAsync("http://localhost:8080/api/Users");
            var result = await client.GetStringAsync("http://localhost:8080/api/Users?UserName=" + users.UserName);
            if (result != "")
            {
                var recvUser = JsonConvert.DeserializeObject<Users>(result);
                string pagePwd = users.Pwd;
                if (recvUser.UserName == users.UserName && recvUser.Pwd.ToLower() == pagePwd.ToLower())
                {
                    HttpContext.Session.SetString("LoginUser", recvUser.UserName);//缓存当前登录用户
                    HttpContext.Session.SetString("LoginUserId", recvUser.UsersId.ToString());//缓存当前登录用户Id
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Login/Create   
        // GET: Register/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Register/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Users user)
        {
            HttpClient client = new HttpClient();

            user.Pwd = user.Pwd;
            var jsonString = JsonConvert.SerializeObject(user);
            HttpContent httpContent = new StringContent(jsonString);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var resMsg = await client.PostAsync("http://localhost:8080/api/Users", httpContent);
            if (resMsg.StatusCode.ToString() == "Created")
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}