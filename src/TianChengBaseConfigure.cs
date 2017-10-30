using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.BaseService.PlugIn;

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
        /// <param name="loggerFactory"></param>
        static public void TianChengInit(this IApplicationBuilder app, IConfigurationRoot configuration,
            ILoggerFactory loggerFactory)
        {
            TianChengInit(app, configuration, loggerFactory, null);
        }

        /// <summary>
        /// 不包含权限处理的初始化操作
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <param name="loggerFactory"></param>
        static public void TianChengInitUnAuth(this IApplicationBuilder app, IConfigurationRoot configuration,
            ILoggerFactory loggerFactory)
        {
            app.UseCors("AllowAnyDomain");

            //允许访问磁盘文件
            app.UseStaticFiles(
            new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            });


            //设置MVC
            app.UseMvc();

            // Swagger
            app.UseSwaggerUI();

            #region Logger
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile("Logs/Runlog-{Date}.txt");
            #endregion
        }

        /// <summary>
        /// Configure 的初始化操作
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="authService"></param>
        static public void TianChengInit(this IApplicationBuilder app, IConfigurationRoot configuration,
            ILoggerFactory loggerFactory, IAuthService authService)
        {
            //跨域处理
            app.UseCors("AllowAnyDomain");
            
            //auth 登录权限控制
            app.AuthConfigure(configuration, authService);

            //允许访问磁盘文件
            app.UseStaticFiles();

            //设置MVC
            app.UseMvc();

            // Swagger
            app.UseSwaggerUI();

            #region Logger
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile("Logs/Demo-{Date}.txt");
            #endregion


        }
    }
}
