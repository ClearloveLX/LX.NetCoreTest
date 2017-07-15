using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LX.NETCoreTest.Model.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using LX.NETCoreTest.Model.PyClass;

namespace LX.NETCoreTest.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {

            //添加数据库上下文
            services.AddDbContext<PyStudio_NetCoreContext>(b =>
            {

                var dbLink = Configuration.GetSection("PySelfSetting:DbLink").Value;
                if (string.IsNullOrWhiteSpace(dbLink)) { throw new Exception("未找到数据库链接。"); }

                b.UseSqlServer(dbLink);

            });

            services.Configure<PySelfSetting>(Configuration.GetSection("PySelfSetting"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
