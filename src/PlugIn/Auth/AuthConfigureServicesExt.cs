using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        static public void AuthConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {

            //注册所有的权限-功能点信息
            services.AddPolicy();


            TokenProviderOptions tokenOptions = TokenProviderOptions.Load(configuration);

            var tokenValidationParameters = new TokenValidationParameters
            {

                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = tokenOptions.SecurityKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = tokenOptions.Issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = tokenOptions.Audience,

                // Validate the token expiry
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                //不使用https
                //o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = tokenValidationParameters;
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {

                        return Task.FromResult(0);
                        //c.NoResult();
                        //c.Response.StatusCode = 401;
                        //return c.Response.WriteAsync("Invalid Token");
                    },
                    OnTokenValidated = context =>
                    {
                        if (context.Principal.Identity != null)
                        {
                            ClaimsIdentity identity = context.Principal.Identity as ClaimsIdentity;

                            string typeName = identity.Claims.Where(e => e.Type == "type").Select(e => e.Value).FirstOrDefault();
                            if (!string.IsNullOrWhiteSpace(typeName))
                            {
                                //var authService = context.Request.HttpContext.RequestServices.GetService<IAuthService>();
                                var authService = TianCheng.Model.ServiceLoader.GetService(typeName) as IAuthService;
                                authService.FillFunctionPolicy(identity);
                            }
                        }

                        return Task.FromResult(0);
                    }
                };
            });






















            //TokenConfigure conf = TokenConfigure.Load();

            ////#region Jwt
            ////string secretKey = configuration.GetValue<string>("TokenSecretKey");
            ////var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(conf.SecretKey));
            ////services.Configure<TianCheng.BaseService.PlugIn.TokenProviderOptions>(options =>
            ////{
            ////    options.Issuer = conf.Issuer;
            ////    options.Audience = conf.Audience;
            ////    options.SigningCredentials = new SigningCredentials(signingKey,SecurityAlgorithms.HmacSha256);
            ////    options.Expiration = conf.Expiration;
            ////});

            ////#endregion Jwt





            //var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(conf.SecretKey));
            //var tokenValidationParameters = new TokenValidationParameters
            //{
            //    // The signing key must match!
            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = signingKey,


            //    // Validate the JWT Issuer (iss) claim
            //    ValidateIssuer = true,
            //    ValidIssuer = conf.Issuer,

            //    // Validate the JWT Audience (aud) claim
            //    ValidateAudience = true,
            //    ValidAudience = conf.Audience,

            //    // Validate the token expiry
            //    //ValidateLifetime = false,

            //};


            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 

            //}).AddJwtBearer(o =>
            //{
            //    o.TokenValidationParameters = tokenValidationParameters;

            //    o.Events = new JwtBearerEvents()
            //    {
            //        OnAuthenticationFailed = context =>
            //        {
            //            if (context.Exception != null && !String.IsNullOrWhiteSpace(context.Exception.Message))
            //            {
            //                if (context.Exception.Message.Contains("IDX14700"))
            //                {
            //                    context.HttpContext.Items["Jwt-AuthenticationFailed"] = null;
            //                }
            //                if (context.Exception.Message.Contains("IDX10729"))
            //                {
            //                    context.HttpContext.Items["Jwt-TokenFailed"] = null;
            //                }
            //                TianCheng.Model.ApiException.ThrowBadRequest(context.Exception.Message);
            //                return Task.FromResult(0);
            //            }

            //            TianCheng.Model.ApiException.ThrowBadRequest("您没有足够的权限进行本次操作。");
            //            return Task.FromResult(0);
            //            //c.NoResult();
            //            //c.Response.StatusCode = 401;
            //            //return c.Response.WriteAsync("Invalid Token");
            //        },
            //        OnTokenValidated = context =>
            //        {
            //            var provider = context.Request.HttpContext.RequestServices;
            //            var authService = provider.GetService<IAuthService>();

            //            if (context.Principal.Identity != null)
            //            {
            //                ClaimsIdentity identity = context.Principal.Identity as ClaimsIdentity;

            //                foreach (var item in identity.Claims)
            //                {
            //                    if (item.Type == "id" && authService != null)
            //                    {
            //                        authService.FillFunctionPolicy(item.Value, identity);
            //                        break;
            //                    }
            //                }
            //            }

            //            return Task.FromResult(0);
            //        }

            //    };
            //});

        }
    }
}
