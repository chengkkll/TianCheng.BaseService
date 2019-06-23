using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TianCheng.BaseService;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 自动注册服务
    /// </summary>
    static public class TianChengBaseConfigureServices
    {
        static private bool IsInit = false;
        /// <summary>
        /// 增加业务的Service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void TianChengBaseServicesInit(this IServiceCollection services, IConfiguration configuration)
        {
            if (IsInit) return;

            TianCheng.Model.ServiceLoader.Services = services;
            TianCheng.Model.ServiceLoader.Configuration = configuration;

            // 注册配置信息
            services.AddOptions();
            // 注册数据库模块配置信息
            services.TianChengDALInit(configuration);

            // 设置跨域
            services.AddCors(options =>
            options.AddPolicy("AllowAnyDomain",
                            builder => builder.AllowAnyOrigin().AllowAnyMethod()
                                              .AllowAnyHeader().AllowAnyOrigin().AllowCredentials()));

            // 根据IServiceRegister 接口来注册能找到的所有服务
            services.AddBusinessServices();
            // 注入请求地址的服务,用于获取访问请求的IP地址信息
            services.TryAddSingleton<AspNetCore.Http.IHttpContextAccessor, AspNetCore.Http.HttpContextAccessor>();

            // 注册MVC及异常的过滤处理
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(TiangCheng.BaseService.PlugIn.ApiExcepiton.ValidateModelAttribute));
                options.Filters.Add(typeof(TiangCheng.BaseService.PlugIn.ApiExcepiton.WebApiExceptionFilterAttribute));    // 增加异常处理
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // 设置对象自动映射
            TianCheng.Model.AutoMapperExtension.InitializeMappers();

            // 查找并配置对所有继承IServiceExtOption接口的对象。实现Service逻辑中的事件效果
            ServiceExtOption.Load(services);

            // swagger
            services.AddMvcCore().AddApiExplorer();
            services.SwaggerRegister();

            // auth 登录权限控制
            services.AuthConfigureServices(configuration);

            IsInit = true;
            TianCheng.Model.CommonLog.Logger.Information("ConfigureServices - TianCheng.BaseService init complete.");
        }
    }
}
