using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// 在Swagger中根据文件上传特性查找，并设置需要文件上传的UI操作
    /// </summary>
    public class SwaggerFileUploadFilter : IOperationFilter
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
                // 如果方法中有文件上传特性。
                if (attr.GetType() != typeof(SwaggerFileUploadAttribute))
                {
                    continue;
                }
                SwaggerFileUploadAttribute upload = (SwaggerFileUploadAttribute)attr;
                if (upload == null)
                {
                    upload = new SwaggerFileUploadAttribute();
                }
                operation.Parameters.Add(new NonBodyParameter()
                {
                    Name = "file",
                    @In = "formData",
                    Description = "准备上传的文件",
                    Required = upload.Required,
                    Type = "file"
                });

            }
        }
    }
}
