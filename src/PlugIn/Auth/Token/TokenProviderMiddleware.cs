using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TianCheng.BaseService.PlugIn
{
    /// <summary>
    /// Jwt 处理的中间件
    /// </summary>
    public class TokenProviderMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// 
        /// </summary>
        private readonly TokenProviderOptions _options;
        /// <summary>
        /// 
        /// </summary>
        //private readonly IUserService _service;
        public IAuthenticationSchemeProvider Schemes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="options"></param>
        /// <param name="schemes"></param>
        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options,
            //IUserService _service, 
            IAuthenticationSchemeProvider schemes)
        {
            _next = next;
            _options = options.Value;
            //this._service = _service;
            Schemes = schemes;
        }

        /// <summary>
        /// invoke the middleware
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            //
            context.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
            {
                OriginalPath = context.Request.Path,
                OriginalPathBase = context.Request.PathBase
            });
            //获取默认Scheme（或者AuthorizeAttribute指定的Scheme）的AuthenticationHandler
            var handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(context, scheme.Name) as IAuthenticationRequestHandler;
                if (handler != null && await handler.HandleRequestAsync())
                {
                    return;
                }
            }
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await context.AuthenticateAsync(defaultAuthenticate.Name);
                if (result?.Principal != null)
                {
                    context.User = result.Principal;
                }
            }
            //
            await _next(context);
            return;
        }
    }
}
