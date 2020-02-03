using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChangeHomeMVC.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChangeHomeMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //依赖注入一个数据库上下文
            string connString = Configuration.GetSection("connString")["ChangeDB"];
            services.AddDbContext<ChangeContext>(options =>
            {
                options.UseSqlServer(connString);
            });

            services.AddSession(options =>      //添加session服务
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);//超时时间,分钟
                options.Cookie.HttpOnly = true;//在cookie中设置了HttpOnly属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击。

                //解释：这是个GDRP条例，让用户自己选择使用用cookie，详见 http://www.zhibin.org/archives/667 或 https://www.cnblogs.com/GuZhenYin/p/9154447.html
                options.Cookie.IsEssential = true;//表示cookie是必须的，否则chrome中拿不到Session值
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
