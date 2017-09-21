using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TianCheng.BaseService.PlugIn
{
    /// <summary>
    /// 生成Token信息的配置
    /// </summary>
    public class TokenConfigure
    {
        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromHours(24);
        /// <summary>
        /// 
        /// </summary>
        public string Scheme { get; set; }
        /// <summary>
        /// 默认的Token配置
        /// </summary>
        static public TokenConfigure Default
        {
            get
            {
                return new TokenConfigure()
                {
                    Issuer = "Eieco.ESP.WebApi",
                    Audience = "Eieco.ESP.Web",
                    SecretKey = "ExhibitionSalesPlatformSecretKey",
                    Expiration = TimeSpan.FromHours(24),
                    Scheme = "Bearer"
                };
            }
        }
        /// <summary>
        /// 判断配置信息是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            if(String.IsNullOrWhiteSpace(SecretKey))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 从配置文件中读取Token信息
        /// </summary>
        /// <returns></returns>
        static public TokenConfigure Load()
        {
            TokenConfigure conf = null;
            try
            {
                var jsonServices = JObject.Parse(File.ReadAllText("appSettings.json"))["Token"];
                conf = JsonConvert.DeserializeObject<TokenConfigure>(jsonServices.ToString());
                if (conf == null || conf.IsEmpty())
                {
                    throw new Exception();
                }
            }
            catch
            {
                conf = TokenConfigure.Default;
            }
            return conf;
        }
    }
}
