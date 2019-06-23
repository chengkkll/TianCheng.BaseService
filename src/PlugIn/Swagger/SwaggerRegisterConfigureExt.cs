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
            // 读取配置信息
            SwaggerDocInfo doc = SwaggerDocInfo.Load();

            #region Swagger
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(doc.EndpointUrl, doc.EndpointDesc);
                // 启用地址路径
                options.EnableDeepLinking();
                // 打开页面时，默认折叠标签
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                // 可以根据标签名称对接口进行过滤
                options.EnableFilter();
                //options.DefaultModelExpandDepth(3);
                //options.DefaultModelsExpandDepth(3);
                //options.ShowRequestHeaders();
            });
            #endregion
        }
    }
}

