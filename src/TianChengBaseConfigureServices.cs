using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
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
        public static void TianChengInit(this IServiceCollection services, IConfigurationRoot configuration)
        {
            if (IsInit) return;
            //设置跨域
            services.AddCors(options =>
            options.AddPolicy("AllowAnyDomain",
                            builder => builder.AllowAnyOrigin().AllowAnyMethod()
                                              .AllowAnyHeader().AllowAnyOrigin().AllowCredentials()));            

            //根据IServiceRegister 接口来注册能找到的服务
            services.AddBusinessServices();

            //注册MVC及异常的过滤处理
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(TiangCheng.BaseService.PlugIn.ApiExcepiton.ValidateModelAttribute));
                options.Filters.Add(typeof(TiangCheng.BaseService.PlugIn.ApiExcepiton.WebApiExceptionFilterAttribute));    //增加异常
            });

            //AuotMapper的对象注册
            TianCheng.BaseService.AutoMapperExtension.InitializeMappers();

            //读取配置信息
            services.AddOptions();
            //services.Configure<FunctionModuleConfig>(configuration.GetSection("FunctionModule"));

            //查找并配置对所有继承IServiceExtOption接口的对象。
            foreach (IServiceExtOption seo in TianCheng.Model.AssemblyHelper.GetInstanceByInterface<IServiceExtOption>())
            {
                seo.SetOption();
            }

            //swagger
            services.SwaggerRegister(configuration);
            
            //注册所有的权限-功能点信息
            services.AddPolicy();            
            //auth 登录权限控制
            services.AuthConfigureServices(configuration);

            IsInit = true;
        }
    }
}
