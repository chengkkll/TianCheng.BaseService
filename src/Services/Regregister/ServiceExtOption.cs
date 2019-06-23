using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 设置/扩展服务
    /// </summary>
    public class ServiceExtOption
    {
        /// <summary>
        /// 加载所有需要扩展的服务
        /// </summary>
        /// <param name="services"></param>
        static public void Load(IServiceCollection services)
        {
            // 查找并配置对所有继承IServiceExtOption接口的对象。实现Service逻辑中的事件效果
            foreach (var type in TianCheng.Model.AssemblyHelper.GetTypeByInterface<IServiceExtOption>()) //.GetInstanceByInterface<IServiceExtOption>())
            {
                foreach (var cons in type.GetConstructors())
                {
                    var pl = cons.GetParameters();
                    if (pl.Length == 1 && pl[0].ParameterType == typeof(IServiceCollection))
                    {
                        object[] parameters = new object[1];
                        parameters[0] = services;

                        IServiceExtOption extOptions = (IServiceExtOption)cons.Invoke(parameters);
                        extOptions.SetOption();
                        continue;
                    }
                    if (pl.Length == 0)
                    {
                        IServiceExtOption extOption = (IServiceExtOption)type.Assembly.CreateInstance(type.FullName);
                        extOption.SetOption();
                        continue;
                    }

                }
            }
        }
    }
}
