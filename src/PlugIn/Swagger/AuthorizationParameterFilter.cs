using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// 权限接口说明设置
    /// </summary>
    public class AuthorizationParameterFilter : IOperationFilter
    {
        private static readonly Response R401 = new Response() { Description = "Token值错误，需要重新登陆。" };
        private static readonly Response R403 = new Response() { Description = "没有权限调用本接口。" };

        /// <summary>
        /// 权限接口说明设置
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context">接口的上下文信息</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();
            // 只查询需要权限的接口
            var authAttrList = context.MethodInfo.GetCustomAttributes(typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute), true);
            if (authAttrList != null && authAttrList.Count() > 0)
            {
                if (!(authAttrList.FirstOrDefault() is Microsoft.AspNetCore.Authorization.AuthorizeAttribute authAttr))
                {
                    return;
                }
                // 设置接口说明
                operation.Description += $"\r\n权限名：\r\n\t{authAttr.Policy}\r\n";
                // 设置接口返回值说明
                if (operation.Responses.Keys.Contains("401"))
                {
                    operation.Responses["401"] = R401;
                }
                if (operation.Responses.Keys.Contains("403"))
                {
                    operation.Responses["403"] = R403;
                }
            }
        }
    }
}
