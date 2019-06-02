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

            // ����û�Session����
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });
            // ָ��Session���淽ʽ:�ַ��ڴ滺��
            services.AddDistributedMemoryCache();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //������ʾ�̬�ļ�
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            //ʹ�þ�̬�ļ�
            app.UseStaticFiles();
            //Cookie����
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
