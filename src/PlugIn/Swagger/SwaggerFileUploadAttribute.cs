using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// 文件上传特性，根据特性标示，在SwaggerUI中显示选择文件操作
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
    public class SwaggerFileUploadAttribute : Attribute
    {
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; private set; }
        /// <summary>
        /// 参数说明
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 文件上传特性,用于Swagger UI 中显示的信息
        /// </summary>
        /// <param name="Required"></param>
        public SwaggerFileUploadAttribute(bool Required = true)
        {
            this.Required = Required;
        }
        /// <summary>
        /// 文件上传特性,用于Swagger UI 中显示的信息
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="description">参数说明</param>
        /// <param name="required">是否必填</param>
        public SwaggerFileUploadAttribute(string name,string description,bool required = true)
        {
            Required = Required;
            Name = name;
            Description = description;
        }
    }
}
