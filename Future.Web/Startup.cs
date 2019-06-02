using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Future.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // 添加用户Session服务
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });
            // 指定Session保存方式:分发内存缓存
            services.AddDistributedMemoryCache();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //允许访问静态文件
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            //使用静态文件
            app.UseStaticFiles();
            //Cookie策略
            app.UseCookiePolicy();
            //Session
            app.UseSession();
            app.UseMvc(routs=> 
            {
                routs.MapRoute(name: "default", template: "{controller=SysTree}/{action=LoginIndex}");
            });
        }
    }
}
