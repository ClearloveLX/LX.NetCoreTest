using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LX.NETCoreTest.Model.Models;
using Microsoft.EntityFrameworkCore;
using LX.NETCoreTest.Model.PyClass;
using Microsoft.AspNetCore;

namespace LX.NETCoreTest.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Add framework services.  
            services.AddMvc();

            //设置自定义配置信息  
            services.Configure<PySelfSetting>(Configuration.GetSection("PySelfSetting"));

            //添加数据库上下文
            services.AddDbContext<PyStudio_NetCoreContext>(b =>
            {

                var dbLink = Configuration.GetSection("PySelfSetting:DbLink").Value;
                if (string.IsNullOrWhiteSpace(dbLink)) { throw new Exception("未找到数据库链接。"); }

                b.UseSqlServer(dbLink);

            });

            //添加cache支持 
            services.AddDistributedMemoryCache();

            //memorycache支持
            services.AddMemoryCache();

            //添加session支持
            services.AddSession(b =>
            {
                b.IdleTimeout = TimeSpan.FromMinutes(60);  //session过期时间
                b.CookieHttpOnly = true;
                b.CookieName = "LoveSid";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
