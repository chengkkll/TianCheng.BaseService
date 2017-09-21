using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 查询所有的策略权限，然后注册
    /// </summary>
    static public class AutoAuthorization
    {
        /// <summary>
        /// 增加业务的Service
        /// </summary>
        /// <param name="services"></param>
        public static void AddPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                foreach (var auth in TianCheng.Model.AssemblyHelper.GetMethodAttribute<AspNetCore.Authorization.AuthorizeAttribute>())
                {
                    options.AddPolicy(auth.Policy, policy => policy.RequireClaim("function_policy", auth.Policy));
                }
            });
        }
    }
}
