using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using TianCheng.BaseService.PlugIn.Crypt;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// 设置接口的加密参数
    /// </summary>
    public class SwaggerDecryptFilter : IOperationFilter
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

            var attrList = context.MethodInfo.GetCustomAttributes(typeof(DecryptBodyAttribute), true);
            if (attrList != null && attrList.Count() > 0)
            {
                if (!(attrList.FirstOrDefault() is DecryptBodyAttribute))
                {
                    return;
                }
                // 设置加密接口参数
                operation.Parameters.Clear();
                operation.Parameters.Add(new BodyParameter()
                {
                    Name = "加密后数据",
                    In = "body",
                    Description = "加密后的数据",
                    Required = true,
                    Schema = new Schema { Type = "string" }
                });
            }
        }
    }
}
