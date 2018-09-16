using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using TianCheng.DAL;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 自动注册服务
    /// </summary>
    static public class ServiceRegister
    {
        /// <summary>
        /// 增加业务的Service
        /// </summary>
        /// <param name="services"></param>
        public static void AddBusinessServices(this IServiceCollection services)
        {
            // 注册Service
            foreach (Type type in TianCheng.Model.AssemblyHelper
                .GetTypeByInterfaceName("TianCheng.BaseService.IServiceRegister"))
            {
                if (type.GetTypeInfo().IsClass)
                {
                    services.AddTransient(type);
                }
            }
            // 注册数据库操作
            foreach (var type in TianCheng.Model.AssemblyHelper.GetTypeByInterfaceName("IDBOperation"))
            {
                if (!type.IsInterface)
                    services.AddTransient(type);
            }

        }
    }
}
