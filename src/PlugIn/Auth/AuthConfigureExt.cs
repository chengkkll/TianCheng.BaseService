﻿using Microsoft.AspNetCore.Builder;

namespace TianCheng.BaseService.PlugIn
{
    /// <summary>
    /// 
    /// </summary>
    static public class AuthConfigureExt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        static public void AuthConfigure(this IApplicationBuilder app)
        {
            //TokenConfigure conf = TokenConfigure.Load();

            //IConfigurationRoot configuration = app.ApplicationServices.GetService<IConfigurationRoot>();
            app.UseAuthentication();


            //TokenConfigure conf = TokenConfigure.Load();
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



            //var audienceConfig = Configuration.GetSection("Audience");
            //var symmetricKeyAsBase64 = audienceConfig["Secret"];
            //var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            //var signingKey = new SymmetricSecurityKey(keyByteArray);
            //var SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            //app.UseAuthentication(new TokenProviderOptions
            //{

            //    Audience = conf.Audience,// audienceConfig["Audience"],
            //    Issuer = conf.Issuer,// audienceConfig["Issuer"],
            //    //SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            //    SigningCredentials = conf.SigningCredentials
            //});


            //if (authService == null)
            //{
            //    foreach (IAuthService auth in TianCheng.Model.AssemblyHelper.GetInstanceByInterface<IAuthService>())
            //    {
            //        if(auth != null)
            //        {
            //            authService = auth;
            //            break;
            //        }
            //    }
            //}

            //先加一层中间件，获取权限错误。
            //app.Use(next => async context => {
            //    try
            //    {
            //        if(context.Request.Headers.ContainsKey("Authorization"))
            //        {
            //            string auth = context.Request.Headers["Authorization"].ToString();
            //            if (!auth.StartsWith("Bearer "))
            //            {
            //                context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
            //                string result = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = 401, message = "Authorization中的Token值错误" });
            //                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(result);

            //                context.Response.Body.Write(byteArray, 0, byteArray.Length);
            //            }
            //        }

            //        await next(context);

            //    }
            //    catch(Exception ex)
            //    {
            //        // If the headers have already been sent, you can't replace the status code.
            //        // In this case, re-throw the exception to close the connection.
            //        if (context.Response.HasStarted)
            //        {
            //            throw;
            //        }

            //        // Rethrow the exception if it was not caused by IdentityModel.
            //        if (context.Items.ContainsKey("Jwt-AuthenticationFailed"))
            //        {
            //            context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
            //            string result = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = 401, message = "您没有足够的权限进行本次操作。" });
            //            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(result);
            //            context.Response.Body.Write(byteArray, 0, byteArray.Length);
            //        }

            //        else if (context.Items.ContainsKey("Jwt-TokenFailed"))
            //        {
            //            context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
            //            string result = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = 401, message = "传递的Token值错误。" });
            //            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(result);
            //            context.Response.Body.Write(byteArray, 0, byteArray.Length);
            //        }
            //        else
            //        {
            //            //string msg = context.Response...Exception.Message;
            //            //context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            //            //string result = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = 400, message = msg });
            //            //byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(result);
            //            //context.Response.Body.Write(byteArray, 0, byteArray.Length);
            //        }


            //    }
            //});


            //#region Jwt
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
            //    ValidateLifetime = false,
            //};
            //app.UseJwtBearerAuthentication(new JwtBearerOptions
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    AuthenticationScheme = conf.Scheme,
            //    TokenValidationParameters = tokenValidationParameters,
            //    Events = new JwtBearerEvents()
            //    {
            //        OnTokenValidated = context =>
            //        {
            //            Claim claim = context.Ticket.Principal.FindFirst(ClaimTypes.NameIdentifier);
            //            if (claim != null)
            //            {
            //                ClaimsIdentity identity = context.Ticket.Principal.Identity as ClaimsIdentity;

            //                authService.FillFunctionPolicy(claim.Value, identity);
            //            }
            //            return Task.FromResult(0);
            //        },
            //        OnAuthenticationFailed = context =>
            //        {
            //            if(context.Exception!= null && !String.IsNullOrWhiteSpace(context.Exception.Message))
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
            //        }

            //    }
            //});

            //#endregion Jwt
        }
    }
}
