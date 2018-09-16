using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TianCheng.BaseService;
using TianCheng.BaseService.Services;

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
        public static void TianChengInit(this IServiceCollection services, IConfiguration configuration)
        {
            // IServiceProvider serviceProvider = services.BuildServiceProvider();
            // IConfigurationRoot configuration = services.BuildServiceProvider().GetService<IConfigurationRoot>();

            if (IsInit) return;

            // ServiceLoader.Services = services;
            TianCheng.DAL.Interface.Appsettings.Services = services;
            TianCheng.Model.ServiceLoader.Services = services;

            TianCheng.Model.CommonLog.Logger.LogInformation("ConfigureServices - TianCheng.BaseService");
            //设置跨域
            services.AddCors(options =>
            options.AddPolicy("AllowAnyDomain",
                            builder => builder.AllowAnyOrigin().AllowAnyMethod()
                                              .AllowAnyHeader().AllowAnyOrigin().AllowCredentials()));

            //根据IServiceRegister 接口来注册能找到的服务
            services.AddBusinessServices();
            services.TryAddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            //注册MVC及异常的过滤处理
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(TiangCheng.BaseService.PlugIn.ApiExcepiton.ValidateModelAttribute));
                options.Filters.Add(typeof(TiangCheng.BaseService.PlugIn.ApiExcepiton.WebApiExceptionFilterAttribute));    //增加异常
            });

            //AuotMapper的对象注册
            TianCheng.BaseService.AutoMapperExtension.InitializeMappers();
            //初始化数据库注册信息
            TianCheng.DAL.MongoDB.Register.Init();

            //读取配置信息
            services.AddOptions();
            //services.Configure<FunctionModuleConfig>(configuration.GetSection("FunctionModule"));

            //查找并配置对所有继承IServiceExtOption接口的对象。
            foreach (var type in TianCheng.Model.AssemblyHelper.GetTypeByInterface<IServiceExtOption>()) //.GetInstanceByInterface<IServiceExtOption>())
            {
                foreach (var cons in type.GetConstructors())
                {
                    var pl = cons.GetParameters();
                    if (pl.Length == 1 && pl[0].ParameterType == typeof(IServiceCollection))
                    {
                        object[] parameters = new object[1];
                        parameters[0] = services;

                        IServiceExtOption extOptions = (IServiceExtOption)cons.Invoke(parameters);
                        extOptions.SetOption();
                        continue;
                    }
                    if (pl.Length == 0)
                    {
                        IServiceExtOption extOption = (IServiceExtOption)type.Assembly.CreateInstance(type.FullName);
                        extOption.SetOption();
                        continue;
                    }

                }
            }

            //swagger
            services.SwaggerRegister();

            //auth 登录权限控制
            services.AuthConfigureServices(configuration);

            IsInit = true;
        }
    }
}
