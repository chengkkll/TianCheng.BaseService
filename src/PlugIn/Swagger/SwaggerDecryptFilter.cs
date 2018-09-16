using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
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

            foreach (var attr in context.ApiDescription.ActionAttributes())
            {
                // 如果方法中有加密特性。
                if (attr.GetType() != typeof(DecryptBodyAttribute))
                {
                    continue;
                }

                // 增加加密属性
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
