using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TianCheng.BaseService.PlugIn.Crypt
{
    /// <summary>
    /// 
    /// </summary>
    public class DecryptBodyAttribute : Attribute, IResourceFilter
    {
        /// <summary>
        /// 请求资源时的操作
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // 修改Body的值
            // 1、读取提交的Body信息
            string body = new StreamReader(context.HttpContext.Request.Body).ReadToEnd();
            if (!String.IsNullOrEmpty(body))
            {
                // 2、解密Body中的数据
                // e.g. : body = "{\"account\" : \"a\",\"password\" :\"p\"}";
                body = RSAHelper.RSADecrypt(CryptConfig.RSA_PRIVATE_KEY, body);
                // 3、将解密后的数据写回Body中
                byte[] requestData = Encoding.UTF8.GetBytes(body);
                context.HttpContext.Request.Body = new MemoryStream(requestData);
                context.HttpContext.Request.Body.Position = 0;
            }
        }
        /// <summary>
        /// 请求资源后的操作
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }
    }
}
