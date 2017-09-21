using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TianCheng.BaseService.PlugIn;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    static public class AuthConfigureServicesExt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        static public void AuthConfigureServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            TokenConfigure conf = TokenConfigure.Load();

            #region Jwt
            //string secretKey = configuration.GetValue<string>("TokenSecretKey");
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(conf.SecretKey));
            services.Configure<TianCheng.BaseService.PlugIn.TokenProviderOptions>(options =>
            {
                options.Issuer = conf.Issuer;
                options.Audience = conf.Audience;
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                options.Expiration = conf.Expiration;
            });

            #endregion Jwt
        }
    }
}
