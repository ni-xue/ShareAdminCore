using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Facade;
using Admin.Facade.Helper;
using Admin.Facade.PermissionsMenu;
using Admin.Facade.Session;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tool.Utils;
using Tool.Web;
using Tool.Web.Builder;
using Tool.Web.Session;

namespace AdminCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region 自定义Session 这里使用了长连接，接入了其他服务，改成可以直接独立运行的版本。

            //ShellSession.InitSession<RedisSession>();

            //services.AddDiySession(c =>
            //{
            //    c.GetDiySession<RedisSession>();
            //});

            //services.AddObject(TcpFrame.CreateServer(AppSettings.Get("ServerIp"), 888));

            #endregion

            #region 自定义Session 独立运行版本，0.0.1

            ShellSession.InitSession<CacheSession>();

            services.AddDiySession(c =>
            {
                c.GetDiySession<CacheSession>();
            });

            CacheSession.StartKeep(1); // 每秒一次自检测

            #endregion

            services.AddResponseCompression();

            services.AddAshx(o =>
            {
                //o.IsAsync = true;
                o.JsonOptions = new()// System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web)
                {
                    //IgnoreReadOnlyFields = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All),
                };

                //o.JsonOptions.Converters.Add(JsonConverterHelper.GetDateConverter());
                o.JsonOptions.Converters.Add(JsonConverterHelper.GetDBNullConverter());
            });//注册api。
               //.AddHttpContext();//注册静态方式的HttpContext对象获取。

            services.AddObject(new UpLoad());//上传有关配置注册
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(AllException);
            }

            app.UseIgnoreUrl("Views");

            app.UseResponseCompression();

            // 此部分为作者需要，你们可以直接删掉
            var staticfile = new StaticFileOptions();
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            provider.Mappings.Add(".php", "text/plain");//手动设置对应MIME 
            provider.Mappings.Add(".aspx", "text/plain");
            staticfile.ContentTypeProvider = provider;
            //staticfile.OnPrepareResponse = (a) => 
            //{
                
            //};
            app.UseStaticFiles(staticfile);

            app.UseDiySession();

            app.UseAshx(routes =>
            {
                routes.MapApiRoute(
                   name: "Api",
                   areaName: "AdminCore.Api",
                   template: "Api/{controller=AdminServers}/{action}/{id?}");

                routes.MapApiRoute(
                    name: "Admin",
                    areaName: "AdminCore.WebUi",
                    template: "{controller=Admin}/{action=Index}/{id?}");
            });

            FacadeManage.UseSqlLog(loggerFactory); //注册相关SQL日志。

            Menu.Reload(); //获取默认系统菜单

            TcpFrame.ConnectClient(loggerFactory);

            app.GetObject<UpLoad>().SetBasePath(env.WebRootPath);
        }
        public async Task AllException(HttpContext context, Exception exception)
        {
            await context.Response.WriteAsync("An unknown error has occurred!");
            Log.Error("捕获全局异常：", exception);//"Log/Risk/"
        }
    }
}
