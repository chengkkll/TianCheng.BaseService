using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;

namespace TianCheng.BaseService.PlugIn
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenProviderOptions
    {
        /// <summary>
        /// 请求路径
        /// </summary>
        public string Path { get; set; } = "/api/auth/login";
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
        public SecurityKey SecurityKey { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromHours(24);
        /// <summary>
        /// 
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        static private TokenProviderOptions _config = null;
        /// <summary>
        /// 读取Token的配置信息
        /// </summary>
        /// <param name="Configuration"></param>
        static public TokenProviderOptions Load(IConfiguration Configuration)
        {
            if (_config == null)
            {
                _config = new TokenProviderOptions();
                var node = Configuration.GetSection("Token");
                _config.Audience = node["Audience"];
                _config.SecretKey = node["SecretKey"];
                _config.Issuer = node["Issuer"];
                var keyByteArray = System.Text.Encoding.ASCII.GetBytes(_config.SecretKey);
                _config.SecurityKey = new SymmetricSecurityKey(keyByteArray);

                _config.SigningCredentials = new SigningCredentials(_config.SecurityKey, SecurityAlgorithms.HmacSha256);

            }
            return _config;
        }
    }
}
