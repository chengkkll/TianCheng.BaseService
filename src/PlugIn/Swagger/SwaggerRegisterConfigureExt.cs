using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TianCheng.BaseService.PlugIn.Swagger;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    static public class SwaggerRegisterConfigureExt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        static public void UseSwaggerUI(this IApplicationBuilder app)
        {
            //读取配置信息
            SwaggerDocInfo doc = null;
            try
            {
                var jsonServices = JObject.Parse(File.ReadAllText("appSettings.json"))["SwaggerDoc"];
                doc = JsonConvert.DeserializeObject<SwaggerDocInfo>(jsonServices.ToString());
                if (doc == null)
                {
                    throw new Exception();
                }
            }
            catch
            {
                doc = SwaggerDocInfo.Default;
            }

            #region Swagger
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(doc.EndpointUrl, doc.EndpointDesc);
                options.ShowRequestHeaders();
                // 增加定制的JS文件
                options.InjectOnCompleteJavaScript("/swagger-custom-ui/on-complete.js");
            });

            // 设置从嵌入资源中读取文件
            app.UseFileServer(new FileServerOptions()
            {
                RequestPath = "/swagger-custom-ui",
                EnableDefaultFiles = true,
                FileProvider = new SwaggerCustomFileProvider()
            });
            #endregion
        }
    }
}

