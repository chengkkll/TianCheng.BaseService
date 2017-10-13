using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TianCheng.BaseService;

namespace SamplesWebApi.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TestServiceOptionExt : IServiceExtOption
    {
        /// <summary>
        /// 
        /// </summary>
        private IServiceCollection _Service;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="service"></param>
        public TestServiceOptionExt(IServiceCollection service)
        {
            //IServiceProvider serviceProvider
            _Service = service;
        }

        /// <summary>
        /// 设置扩展
        /// </summary>
        public void SetOption()
        {


        }
    }
}
