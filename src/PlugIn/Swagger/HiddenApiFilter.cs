using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// 阻止API在Swagger中输出特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public partial class HiddenApiAttribute : Attribute { }
    /// <summary>
    /// 隐藏API接口输出过滤
    /// </summary>
    public class HiddenApiFilter : IDocumentFilter
    {

        /// <summary>  
        /// 重写Apply方法，移除隐藏接口的生成  
        /// </summary>  
        /// <param name="swaggerDoc">swagger文档文件</param>  
        /// <param name="context">api接口集合</param>  
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            //按特新隐藏接口
            foreach (ApiDescription apiDescription in context.ApiDescriptionsGroups.Items.SelectMany(e => e.Items))
            {
                //按Action 隐藏接口
                if (apiDescription.ActionAttributes().OfType<HiddenApiAttribute>().Count() > 0)
                {
                    var key = apiDescription.RelativePath.TrimEnd('/');

                    foreach (var path in swaggerDoc.Paths.Where(e => e.Key.Contains(key)))
                    {
                        switch (apiDescription.HttpMethod.ToUpper())
                        {
                            case "OPTIONS":
                                {
                                    path.Value.Options = null; break;
                                }
                            case "POST":
                                {
                                    path.Value.Post = null; break;
                                }
                            case "PUT":
                                {
                                    path.Value.Put = null; break;
                                }
                            case "GET":
                                {
                                    path.Value.Get = null; break;
                                }
                            case "PATCH":
                                {
                                    path.Value.Patch = null; break;
                                }
                            case "DELETE":
                                {
                                    path.Value.Delete = null; break;
                                }
                        }
                    }
                }

                //按Controller 隐藏接口
                if (apiDescription.ControllerAttributes().OfType<HiddenApiAttribute>().Count() > 0)
                {
                    var key = apiDescription.RelativePath.TrimEnd('/');
                    if (swaggerDoc.Paths.ContainsKey(key))
                        swaggerDoc.Paths.Remove(key);
                }

            }

            //默认隐藏Options接口
            foreach (var path in swaggerDoc.Paths)
            {
                path.Value.Options = null;
            }
        }

    }
}
