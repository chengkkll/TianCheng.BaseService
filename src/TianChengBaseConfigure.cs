using Microsoft.Extensions.Configuration;
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
        static public void TianChengBaseServicesInit(this IApplicationBuilder app, IConfiguration configuration)
        {
            TianCheng.Model.ServiceLoader.Instance = app.ApplicationServices;
            // 初始化数据库模块
            TianCheng.DAL.LoadDB.Init();
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
