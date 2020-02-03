using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ChangeMvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace ChangeMvc.Controllers
{
    public class CarController : Controller
    {
        // GET: Car
        public async Task<ActionResult> Index()//购物车页面显示
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

            var resultCat = await httpClient.GetStringAsync("http://localhost:8080/api/Cars/" + HttpContext.Session.GetString("LoginUserId"));
            IEnumerable<Car> listCat = JsonConvert.DeserializeObject<IEnumerable<Car>>(resultCat);
            return View(listCat);
        }

        // GET: Car/Details/5
        public async Task<ActionResult> Details(int ProductId)//收藏加入购物车
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUserId")))
            {
                return RedirectToAction("Index", "Login");
            }
            HttpClient client = new HttpClient();
            var resultCar = await client.GetStringAsync("http://localhost:8080/api/Cars/" + HttpContext.Session.GetString("LoginUserId"));
            IEnumerable<Car> listCat = JsonConvert.DeserializeObject<IEnumerable<Car>>(resultCar);
            var carProduct = listCat.Where(p => p.ProductId == ProductId).ToList();
            if (carProduct.Count == 0)
            {
                var resultP = await client.GetStringAsync("http://localhost:8080/api/Products/" + ProductId);
                Product product = JsonConvert.DeserializeObject<Product>(resultP);

                var resultPh = await client.GetStringAsync("http://localhost:8080/api/Photos");
                IEnumerable<Photo> listPh = JsonConvert.DeserializeObject<IEnumerable<Photo>>(resultPh);
                listPh = listPh.Where(p => p.ProductId == ProductId).ToList();

                Car car = new Car()
                {
                    ProductId = ProductId,
                    AddDay = DateTime.Now,
                    ProductNum = 1,
                    UsersId = int.Parse(HttpContext.Session.GetString("LoginUserId")),
                    Title = product.Title,
                    Price = product.Price,
                    PhotoUrl = listPh.ToList()[0].PhotoUrl,
                };

                var jsonString = JsonConvert.SerializeObject(car);
                HttpContent httpContent = new StringContent(jsonString);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var resMsg = await client.PostAsync("http://localhost:8080/api/Cars", httpContent);
            }
            //if (resMsg.StatusCode.ToString() == "Created")
            //{
            //    return RedirectToAction("Index");
            //}
            return RedirectToAction("Index");

        }

        // GET: Car/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Car/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Car/Edit/5
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

        // GET: Car/Delete/5
        public async Task<IActionResult> Delete(int id)//从购物车里删除商品
        {
            HttpClient client = new HttpClient();
            var resMsg = await client.DeleteAsync("http://localhost:8080/api/Cars/" + id);

            return RedirectToAction("Index");
        }

        // POST: Car/Delete/5
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
        [HttpGet]
        public ActionResult CheckOrderInfo()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> CheckOrderInfo(IFormCollection Form /*表单集合传参*/)//填写核对订单信息
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUserId")))
            {
                return RedirectToAction("Index", "Login");
            }
            if (Form["ProductId"].Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUser")))
            {
                ViewBag.LoginName = HttpContext.Session.GetString("LoginUser");//显示界面当前登录用户
                ViewBag.LoginId = HttpContext.Session.GetString("LoginUserId");//显示界面当前登录用户
            }
            List<Car> cars = new List<Car>();
            for (int i = 0; i < Form["ProductId"].Count; i++)
            {
                Car car = new Car();
                car.ProductId = int.Parse(Form["ProductId"][i]);
                car.PhotoUrl = Form["PhotoUrl"][i];
                car.Title = Form["Title"][i];
                car.ProductNum = int.Parse(Form["ProductNum"][i]);
                car.Price = decimal.Parse(Form["Price"][i]);

                cars.Add(car);
            }
            ViewBag.Total = 0;
            foreach (var item in cars)
            {
                ViewBag.Total += item.ProductNum * item.Price;
            }
            HttpClient client = new HttpClient();
            var resultC = await client.GetStringAsync("http://localhost:8080/api/Categories");
            IEnumerable<Category> listC = JsonConvert.DeserializeObject<IEnumerable<Category>>(resultC);
            ViewData["Category"] = listC.Where(p => p.ParentId != p.CategoryId);

            var resultD = await client.GetStringAsync("http://localhost:8080/api/Deliveries/" + HttpContext.Session.GetString("LoginUserId"));
            IEnumerable<Delivery> listD = JsonConvert.DeserializeObject<IEnumerable<Delivery>>(resultD);
            ViewData["Delivery"] = listD;

            return View(cars);
        }
        [HttpGet]
        public ActionResult SubmitOrder()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> SubmitOrder(IFormCollection Form /*表单集合传参*/)//提交订单
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUserId")))
            {
                return RedirectToAction("Index", "Login");
            }
            if (Form["ProductId"].Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LoginUser")))
            {
                ViewBag.LoginName = HttpContext.Session.GetString("LoginUser");//显示界面当前登录用户
                ViewBag.LoginId = HttpContext.Session.GetString("LoginUserId");//显示界面当前登录用户
            }
            HttpClient client = new HttpClient();
            var resultC = await client.GetStringAsync("http://localhost:8080/api/Categories");
            IEnumerable<Category> listC = JsonConvert.DeserializeObject<IEnumerable<Category>>(resultC);
            ViewData["Category"] = listC.Where(p => p.ParentId != p.CategoryId);

            Orders order = new Orders();
            //  [OrdersId]
            //,[Orderdate]
            //,[UsersId]
            //,[Total]
            //,[DeliveryId]
            //,[DeliveryDate]
            //,[States]
            //,[Remark]
            order.OrdersId = int.Parse(Form["OrdersId"][0]);
            order.Orderdate =DateTime.Parse( DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            order.UsersId = int.Parse(HttpContext.Session.GetString("LoginUserId"));
            order.Total = decimal.Parse(Form["Total"][0]);
            order.DeliveryId = int.Parse(Form["DeliveryId"][0]);//用户收货地址编号
            order.States = 0;
            order.Remark = Form["Remark"][0];

            var jsonString = JsonConvert.SerializeObject(order);
            HttpContent httpContent = new StringContent(jsonString);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var resMsg = await client.PostAsync("http://localhost:8080/api/Orders", httpContent);

            ViewBag.OrdersId = order.OrdersId;
            ViewBag.Total = order.Total;
            //List<OrdersDetail> listOD = new List<OrdersDetail>();
            //  [OrdersId]
            //,[ProductId]
            //,[Quantity]
            //,[States]
            //,[PhotoUrl]
            //,[Title]
            //,[Price]
            for (int i = 0; i < Form["ProductId"].Count; i++)
            {
                OrdersDetail od = new OrdersDetail();
                od.OrdersId = order.OrdersId;
                od.ProductId = int.Parse(Form["ProductId"][i]);
                od.Quantity = int.Parse(Form["ProductNum"][i]);
                od.States = 0;//0正常，1退货中，2已退货                
                od.PhotoUrl = Form["PhotoUrl"][i];
                od.Title = Form["Title"][i];
                od.Price = decimal.Parse(Form["Price"][i]);
                //listOD.Add(od);
                jsonString = JsonConvert.SerializeObject(od);
                httpContent = new StringContent(jsonString);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                resMsg = await client.PostAsync("http://localhost:8080/api/OrdersDetails", httpContent);

                //清除购物车里面的商品
                resMsg = await client.DeleteAsync("http://localhost:8080/api/Cars/" + od.ProductId);

            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> OrderPay(string OrdersId)//订单支付
        {
            HttpClient client = new HttpClient();
            var resultO = await client.GetStringAsync("http://localhost:8080/api/Orders?OrderId="+OrdersId);
            Orders order = JsonConvert.DeserializeObject<Orders>(resultO);
            order.States = 1;
            
            var jsonString = JsonConvert.SerializeObject(order);
            HttpContent httpContent = new StringContent(jsonString);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var resMsg = await client.PutAsync("http://localhost:8080/api/Orders/"+ OrdersId, httpContent);

            return RedirectToAction("MyCenter", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> ReceiveGoods(string OrdersId)//已收货
        {
            HttpClient client = new HttpClient();
            var resultO = await client.GetStringAsync("http://localhost:8080/api/Orders?OrderId=" + OrdersId);
            Orders order = JsonConvert.DeserializeObject<Orders>(resultO);
            order.States = 3;
            order.DeliveryDate = DateTime.Now;

            var jsonString = JsonConvert.SerializeObject(order);
            HttpContent httpContent = new StringContent(jsonString);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var resMsg = await client.PutAsync("http://localhost:8080/api/Orders/" + OrdersId, httpContent);

            return RedirectToAction("MyCenter", "Home");
        }
    }
}