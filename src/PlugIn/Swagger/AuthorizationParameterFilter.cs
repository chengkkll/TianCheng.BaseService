using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// 登录Token参数设置
    /// </summary>
    public class AuthorizationParameterFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            foreach (var attr in context.ApiDescription.ActionAttributes())
            {
                // 如果方法中有权限验证的特性。
                if (attr.GetType() != typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute))
                {
                    continue;
                }
                Microsoft.AspNetCore.Authorization.AuthorizeAttribute upload = (Microsoft.AspNetCore.Authorization.AuthorizeAttribute)attr;
                if (upload == null)
                {
                    upload = new Microsoft.AspNetCore.Authorization.AuthorizeAttribute();
                }
                operation.Parameters.Add(new NonBodyParameter()
                {
                    Name = "Authorization",
                    @In = "header",
                    Description = $"JWT的Token验证\r\n\t【需要Bearer token的形式。\r\n\tBearer与Token之间需一个空格连接】",
                    Required = false,
                    Type = "string",
                });
                operation.Summary += "[Auth]";
                operation.Description = operation.Description + $"\r\n#####权限名称：\r\n\t{upload.Policy}\r\n";


            }
        }
    }
}
