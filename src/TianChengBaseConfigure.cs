using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.BaseService.PlugIn;
using System.Linq;
using TianCheng.BaseService.Services;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    static public class TianChengBaseConfigure
    {
        /// <summary>
        /// Configure 的初始化操作
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        static public void TianChengInit(this IApplicationBuilder app, IConfiguration configuration)
        {
            // ServiceLoader.Instance = app.ApplicationServices;
            TianCheng.DAL.Interface.Appsettings.Instance = app.ApplicationServices;
            TianCheng.DAL.Interface.Appsettings.Configuration = configuration;
            TianCheng.Model.ServiceLoader.Instance = app.ApplicationServices;

            //跨域处理
            app.UseCors("AllowAnyDomain");

            //auth 登录权限控制
            app.AuthConfigure();

            //允许访问磁盘文件
            app.UseStaticFiles();

            //设置MVC
            app.UseMvc();

            // Swagger
            app.UseSwaggerUI();


        }
    }
}
