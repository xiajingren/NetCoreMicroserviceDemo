using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.MVC.Helper;

namespace Web.MVC
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
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "http://apigateway:9070/auth";//通过网关访问鉴权中心
                    //options.Authority = "http://auth:9080";

                    options.ClientId = "web client";
                    options.ClientSecret = "web client secret";
                    options.ResponseType = "code";

                    options.RequireHttpsMetadata = false;

                    options.SaveTokens = true;

                    options.Scope.Add("orderApiScope");
                    options.Scope.Add("productApiScope");
                });

            services.AddControllersWithViews();
            
            //注入IServiceHelper
            //services.AddSingleton<IServiceHelper, ServiceHelper>();
            
            //注入IServiceHelper
            services.AddSingleton<IServiceHelper, GatewayServiceHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceHelper serviceHelper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //程序启动时 获取服务列表
            //serviceHelper.GetServices();
        }
    }
}
