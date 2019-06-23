using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// 为Swagger文档中增加通用注释
    /// </summary>
    public class AppendComment : IDocumentFilter
    {
        /// <summary>  
        /// 为Swagger文档中增加通用注释
        /// </summary>  
        /// <param name="swaggerDoc">swagger文档文件</param>  
        /// <param name="context">api接口集合</param>  
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            //默认隐藏Options接口
            foreach (var path in swaggerDoc.Paths)
            {
                var control = path.Value;

                SetResponse(control);
            }
        }

        #region 异常说明的返回
        private static readonly Response R404 = new Response() { Description = "请求失败。返回信息结构：{ code:\"404\",message:\"错误信息\" }" };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        private void SetResponse(PathItem control)
        {
            if (control == null)
            {
                return;
            }
            SetResponse(control.Get);
            SetResponse(control.Post);
            SetResponse(control.Put);
            SetResponse(control.Patch);
            SetResponse(control.Options);
            SetResponse(control.Delete);
        }
        /// <summary>
        /// 为一个接口设置默认的返回信息
        /// </summary>
        /// <param name="oper"></param>
        private void SetResponse(Operation oper)
        {
            if (oper == null)
            {
                return;
            }
            // 为返回值信息添加常见的异常返回信息
            if (oper.Responses == null)
            {
                oper.Responses = new Dictionary<string, Response>();
            }
            if (!oper.Responses.Keys.Contains("404"))
            {
                oper.Responses.Add("404", R404);
            }
        }
        #endregion
    }
}
