using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerDocInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TermsOfService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContactEmail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContactUrl { get; set; }
        /// <summary>
        /// 读取的XML文件相对运行的位置
        /// </summary>
        public string XmlPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EndpointUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EndpointDesc { get; set; }

        /// <summary>
        /// 默认文档内容
        /// </summary>
        static public SwaggerDocInfo Default
        {
            get
            {
                SwaggerDocInfo doc = new SwaggerDocInfo();

                doc.Version = "v1";
                doc.Title = "演示系统接口文档";
                doc.Description = "RESTful API for 演示系统接";
                doc.TermsOfService = "None";
                doc.ContactName = "cheng";
                doc.ContactEmail = "cheng_kkll@163.com";
                doc.ContactUrl = "";
                doc.EndpointUrl = "/swagger/v1/swagger.json";
                doc.EndpointDesc = "演示系统 API V1";
                doc.XmlPath = "App_Data";

                return doc;
            }
        }
    }
}
