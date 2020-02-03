using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChangeMvc
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
            services.AddSession(options =>      //���session����
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);//��ʱʱ��,����
                options.Cookie.HttpOnly = true;//��cookie��������HttpOnly���ԣ���ôͨ��js�ű����޷���ȡ��cookie��Ϣ����������Ч�ķ�ֹXSS������

                //���ͣ����Ǹ�GDRP���������û��Լ�ѡ��ʹ����cookie����� http://www.zhibin.org/archives/667 �� https://www.cnblogs.com/GuZhenYin/p/9154447.html
                options.Cookie.IsEssential = true;//��ʾcookie�Ǳ���ģ�����chrome���ò���Sessionֵ
            });
            services.AddSingleton<IDistributedCache>(//��ȡ�����ļ�
                ServiceProvider => new RedisCache(new RedisCacheOptions //����Session��ͨ��Session��Redis
                {
                    Configuration = Configuration.GetSection("RedisDB:Connstring").Value,
                    InstanceName = Configuration.GetSection("RedisDB:Instance").Value,
                })
                );
            //�ֲ�ʽ������������ͬ�ĻỰ��ʶ
            services.AddDataProtection(options => {
                options.ApplicationDiscriminator = "lrk.com";
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
