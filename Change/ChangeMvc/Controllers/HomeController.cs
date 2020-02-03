using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using ChangeMvc.Models;

namespace ChangeMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }
        //首页加载显示信息
        public async Task<IActionResult> Index(int? CategoryID = 0, int? ProductID = 0, string Exit = null)
        {
            if (Exit == "Exit")
            {
                HttpContext.Session.Remove("LoginUser");
                HttpContext.Session.Remove("LoginUserId");
            }
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUser")))
            {
                ViewBag.LoginName = HttpContext.Session.GetString("LoginUser");//显示界面当前登录用户
                ViewBag.LoginId = HttpContext.Session.GetString("LoginUserId");//显示界面当前登录用户
            }
            if (CategoryID == 0 && ProductID == 0)
            {
                HttpClient httpClient = new HttpClient();
                var resultC = await httpClient.GetStringAsync("http://localhost:8080/api/Categories");
                IEnumerable<Category> listC = JsonConvert.DeserializeObject<IEnumerable<Category>>(resultC);
                ViewData["Category"] = listC.Where(p => p.ParentId != p.CategoryId);

                var resultP = await httpClient.GetStringAsync("http://localhost:8080/api/Products");
                IEnumerable<Product> listP = JsonConvert.DeserializeObject<IEnumerable<Product>>(resultP);

                var resultPh = await httpClient.GetStringAsync("http://localhost:8080/api/Photos");
                IEnumerable<Photo> listPh = JsonConvert.DeserializeObject<IEnumerable<Photo>>(resultPh);

                var result = (from p in listP
                              join c in listC on p.CategoryId equals c.CategoryId
                              join ph in listPh on p.ProductId equals ph.ProductId
                              select new 
                              {
                                  ProductId = p.ProductId,
                                  CategoryId = c.CategoryId,
                                  ParentId = c.ParentId,
                                  Title = p.Title,
                                  CateName = c.CateName,
                                  PhotoUrl = ph.PhotoUrl,
                                  MarketPrice = p.MarketPrice,
                                  Price = p.Price
                              }).ToList();
                ViewData["CateNameFZ"] = result.Where(p => p.ParentId == 3 && p.CategoryId != p.ParentId).Take(4);//服饰区3
                ViewData["CateNameSM2"] = result.Where(p => p.ParentId == 4 && p.CategoryId != p.ParentId).Take(2);//数码区4
                ViewData["CateNameSM4"] = result.Where(p => p.ParentId == 4 && p.CategoryId != p.ParentId).Skip(2).Take(4);//数码区4
                ViewData["CateNameMS"] = result.Where(p => p.ParentId == 5 && p.CategoryId != p.ParentId).Take(4);//美食区5
                ViewData["CateNameJJ"] = result.Where(p => p.ParentId == 6 && p.CategoryId != p.ParentId).Take(4);//家居区6
                ViewData["CateNameMZ2"] = result.Where(p => p.ParentId == 7 && p.CategoryId != p.ParentId).Take(2);//美妆区7
                ViewData["CateNameMZ1"] = result.Where(p => p.ParentId == 7 && p.CategoryId != p.ParentId).Skip(2).Take(1);//美妆区7
                ViewData["CateNameMY1"] = result.Where(p => p.ParentId == 8 && p.CategoryId != p.ParentId).Take(2);//母婴区8
                ViewData["CateNameMY2"] = result.Where(p => p.ParentId == 8 && p.CategoryId != p.ParentId).Skip(2).Take(2);//母婴区8

            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        //促销/大宗商品/活动咨询
        public async Task<IActionResult> News(string NTypes)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUser")))
            {
                ViewBag.LoginName = HttpContext.Session.GetString("LoginUser");//显示界面当前登录用户
                ViewBag.LoginId = HttpContext.Session.GetString("LoginUserId");//显示界面当前登录用户
            }
            ViewData["NTypes"] = NTypes;
            HttpClient httpClient = new HttpClient();
            var resultC = await httpClient.GetStringAsync("http://localhost:8080/api/Categories");
            IEnumerable<Category> listC = JsonConvert.DeserializeObject<IEnumerable<Category>>(resultC);
            ViewData["Category"] = listC.Where(p => p.ParentId != p.CategoryId);

            var resultN = await httpClient.GetStringAsync("http://localhost:8080/api/News?NTypes=" + NTypes);
            if (resultN != "")
            {
                IEnumerable<News> listN = JsonConvert.DeserializeObject<IEnumerable<News>>(resultN);
                ViewData["News"] = listN;
            }
            else
            {
                ViewData["News"] = null;
            }
            return View();
        }
        //首页查找产品信息
        public async Task<IActionResult> ProductSearch(string Title)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUser")))
            {
                ViewBag.LoginName = HttpContext.Session.GetString("LoginUser");//显示界面当前登录用户
                ViewBag.LoginId = HttpContext.Session.GetString("LoginUserId");//显示界面当前登录用户
            }
            HttpClient httpClient = new HttpClient();
            var resultC = await httpClient.GetStringAsync("http://localhost:8080/api/Categories");
            IEnumerable<Category> listC = JsonConvert.DeserializeObject<IEnumerable<Category>>(resultC);
            ViewData["Category"] = listC.Where(p => p.ParentId != p.CategoryId);

            var resultP = await httpClient.GetStringAsync("http://localhost:8080/api/Products");
            IEnumerable<Product> listP = JsonConvert.DeserializeObject<IEnumerable<Product>>(resultP);
            listP = (from p in listP where (p.Title.Contains(Title)) select p);

            var resultPh = await httpClient.GetStringAsync("http://localhost:8080/api/Photos");
            IEnumerable<Photo> listPh = JsonConvert.DeserializeObject<IEnumerable<Photo>>(resultPh);

            var result = (from p in listP
                          join ph in listPh on p.ProductId equals ph.ProductId
                          select new 
                          {
                              ProductId = p.ProductId,
                              CategoryId = p.CategoryId,
                              Title = p.Title,
                              PhotoUrl = ph.PhotoUrl,
                              MarketPrice = p.MarketPrice,
                              Price = p.Price
                          }).ToList();
            ViewBag.CategoryName = "搜索";
            return View(result);
        }

        //首页显示产品分类信息
        public async Task<IActionResult> ProductCategory(string CategoryName, int CategoryId, int ParentId = 0)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUser")))
            {
                ViewBag.LoginName = HttpContext.Session.GetString("LoginUser");//显示界面当前登录用户
                ViewBag.LoginId = HttpContext.Session.GetString("LoginUserId");//显示界面当前登录用户
            }
            HttpClient httpClient = new HttpClient();
            var resultC = await httpClient.GetStringAsync("http://localhost:8080/api/Categories");
            IEnumerable<Category> listC = JsonConvert.DeserializeObject<IEnumerable<Category>>(resultC);
            ViewData["Category"] = listC.Where(p => p.ParentId != p.CategoryId);

            var resultPh = await httpClient.GetStringAsync("http://localhost:8080/api/Photos");
            IEnumerable<Photo> listPh = JsonConvert.DeserializeObject<IEnumerable<Photo>>(resultPh);

            var resultP = await httpClient.GetStringAsync("http://localhost:8080/api/Products");// + CategoryId);

            if (resultP != "" && CategoryId == ParentId)
            {
                IEnumerable<Product> listP = JsonConvert.DeserializeObject<IEnumerable<Product>>(resultP);
                IEnumerable<Category> listPc = listC.Where(p => p.ParentId == ParentId && p.CategoryId != ParentId);
                var result = (from p in listP
                              join c in listPc on p.CategoryId equals c.CategoryId
                              join ph in listPh on p.ProductId equals ph.ProductId
                              select new 
                              {
                                  ProductId = p.ProductId,
                                  CategoryId = c.CategoryId,
                                  ParentId = c.ParentId,
                                  Title = p.Title,
                                  CateName = c.CateName,
                                  PhotoUrl = ph.PhotoUrl,
                                  MarketPrice = p.MarketPrice,
                                  Price = p.Price
                              }).ToList();
                ViewBag.CategoryName = CategoryName;
                return View(result);
            }
            else if (resultP != "" && CategoryId != ParentId)
            {
                IEnumerable<Product> listP = JsonConvert.DeserializeObject<IEnumerable<Product>>(resultP);
                listP = listP.Where(p => p.CategoryId == CategoryId);
                ViewBag.CategoryName = CategoryName;
                return View(listP);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //查看商品详情
        public async Task<IActionResult> ProductInfo(int ProductId)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUser")))
            {
                ViewBag.LoginName = HttpContext.Session.GetString("LoginUser");//显示界面当前登录用户
                ViewBag.LoginId = HttpContext.Session.GetString("LoginUserId");//显示界面当前登录用户
            }
            HttpClient httpClient = new HttpClient();
            var resultC = await httpClient.GetStringAsync("http://localhost:8080/api/Categories");
            IEnumerable<Category> listC = JsonConvert.DeserializeObject<IEnumerable<Category>>(resultC);
            ViewData["Category"] = listC.Where(p => p.ParentId != p.CategoryId);

            var resultP = await httpClient.GetStringAsync("http://localhost:8080/api/Products/" + ProductId);
            Product product = JsonConvert.DeserializeObject<Product>(resultP);

            return View(product);
        }
        //个人中心
        [HttpGet]
        public async Task<ActionResult> MyCenter(int id)//个人中心页面显示
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUserId")))
            {
                return RedirectToAction("Index", "Login");
            }
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUser")))
            {
                ViewBag.LoginName = HttpContext.Session.GetString("LoginUser");//显示界面当前登录用户
                ViewBag.LoginId = HttpContext.Session.GetString("LoginUserId");//显示界面当前登录用户
            }

            HttpClient httpClient = new HttpClient();
            var resultC = await httpClient.GetStringAsync("http://localhost:8080/api/Categories");
            IEnumerable<Category> listC = JsonConvert.DeserializeObject<IEnumerable<Category>>(resultC);
            ViewData["Category"] = listC.Where(p => p.ParentId != p.CategoryId);

            var resultD = await httpClient.GetStringAsync("http://localhost:8080/api/Deliveries/"+ HttpContext.Session.GetString("LoginUserId"));
            IEnumerable<Delivery> listD = JsonConvert.DeserializeObject<IEnumerable<Delivery>>(resultD);
            ViewData["Delivery"] = listD;//收货地址

            var resultO = await httpClient.GetStringAsync("http://localhost:8080/api/Orders/" + HttpContext.Session.GetString("LoginUserId"));
            IEnumerable<Orders> listO = JsonConvert.DeserializeObject<IEnumerable<Orders>>(resultO);
            ViewData["Orders0"] = listO.Where(p => p.States == 0);//0未付款订单
            ViewData["Orders1"] = listO.Where(p => p.States == 1);//1已付款订单
            ViewData["Orders2"] = listO.Where(p => p.States == 2);//2已发货订单
            ViewData["Orders3"] = listO.Where(p => p.States == 3);//3已收货订单
            ViewData["Orders4"] = listO.Where(p => p.States == 4);//4已评价订单            

            var resultU = await httpClient.GetStringAsync("http://localhost:8080/api/Users?UserName=" + HttpContext.Session.GetString("LoginUser"));
            Users userInfo = JsonConvert.DeserializeObject<Users>(resultU);
            ViewBag.User = userInfo;//用户信息

            var resultA = await httpClient.GetStringAsync("http://localhost:8080/api/Appraises/" + HttpContext.Session.GetString("LoginUserId"));
            IEnumerable<Appraise> listA = JsonConvert.DeserializeObject<IEnumerable<Appraise>>(resultA);
            ViewData["Appraise"] = listA;//评价记录

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
