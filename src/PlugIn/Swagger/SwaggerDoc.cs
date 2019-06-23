using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

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
                return new SwaggerDocInfo
                {
                    Version = "v1",
                    Title = "演示系统接口文档",
                    Description = "RESTful API for 演示系统接",
                    TermsOfService = "None",
                    ContactName = "cheng",
                    ContactEmail = "cheng_kkll@163.com",
                    ContactUrl = "",
                    EndpointUrl = "/swagger/v1/swagger.json",
                    EndpointDesc = "演示系统 API V1",
                    XmlPath = "App_Data"
                };
            }
        }
        /// <summary>
        /// 加载配置文件中的配置信息
        /// </summary>
        /// <returns></returns>
        static public SwaggerDocInfo Load()
        {
            SwaggerDocInfo doc;
            try
            {
                var jsonServices = JObject.Parse(File.ReadAllText("appSettings.json"))["SwaggerDoc"];
                doc = JsonConvert.DeserializeObject<SwaggerDocInfo>(jsonServices.ToString());
                if (doc == null)
                {
                    doc = SwaggerDocInfo.Default;
                }
            }
            catch
            {
                doc = SwaggerDocInfo.Default;
            }
            return doc;
        }

        /// <summary>
        /// 获取模块序号信息
        /// </summary>
        /// <returns></returns>
        static public Dictionary<string, string> ModelIndex()
        {
            try
            {


                var result = new Dictionary<string, string>();
                int index = 10;
                JToken dictJson = JObject.Parse(File.ReadAllText("appSettings.json"))["FunctionModule"]["ModuleDict"];
                foreach (var item in dictJson.Children())
                {
                    string code = item["Code"].ToString();
                    if (!result.ContainsKey(code))
                    {
                        result.Add(code, (index++).ToString());
                    }
                }
                return result;
            }
            catch
            {
                return new Dictionary<string, string>
                {
                    { "AuthController","00" },
                    { "TianCheng.SystemCommon" , "zz"}
                };
            }
        }
    }
}
