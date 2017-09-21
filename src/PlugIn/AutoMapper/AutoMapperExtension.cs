using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutoMapperExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination MapTo<TDestination>(this object source)
        {
            return AutoMapper.Mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 初始化对象映射关系
        /// </summary>
        public static void InitializeMappers()
        {
            var cfg = new AutoMapper.Configuration.MapperConfigurationExpression();

            foreach (Type proType in AssemblyHelper.GetTypeByInterface<TianCheng.Model.IAutoProfile>())
            {
                cfg.AddProfile(proType);
            }

            AutoMapper.Mapper.Initialize(cfg);
        }

        /// <summary>
        /// 检验Mapper 是否正确
        /// </summary>
        /// <param name="logger"></param>
        public static void AssertMapper(ILogger logger)
        {
            try
            {
                AutoMapper.Mapper.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
            }
        }

    }

}
