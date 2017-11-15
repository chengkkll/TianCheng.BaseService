using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoggerExt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingContext"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static void AddCommonLog(WebHostBuilderContext hostingContext, ILoggingBuilder builder)
        {
            builder.AddFile(hostingContext.Configuration.GetSection("Logging"));
        }

    }
}