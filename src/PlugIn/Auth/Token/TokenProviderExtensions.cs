using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace TianCheng.BaseService.PlugIn
{
    /// <summary>
    /// 
    /// </summary>
    public static class TokenProviderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var config = app.ApplicationServices.GetService<IConfiguration>();

            TokenProviderOptions tokenOptions = TokenProviderOptions.Load(config);

            return app.UseMiddleware<TokenProviderMiddleware>(Options.Create(tokenOptions));
        }
    }
}
