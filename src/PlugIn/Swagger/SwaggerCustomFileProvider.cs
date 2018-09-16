using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// Swagger 定制文件服务
    /// </summary>
    public class SwaggerCustomFileProvider : IFileProvider
    {
        #region 属性及构造方法
        private const string EmbeddedFilesNamespace = "TianCheng.BaseService.PlugIn.Swagger.UI";  // 嵌入文件所在命名空间
        private readonly Assembly _thisAssembly;                                                  // 当前程序集
        private readonly EmbeddedFileProvider _staticFileProvider;                                // 嵌入文件读取服务

        /// <summary>
        /// 构造方法
        /// </summary>
        public SwaggerCustomFileProvider()
        {
            _thisAssembly = GetType().GetTypeInfo().Assembly;
            _staticFileProvider = new EmbeddedFileProvider(_thisAssembly, EmbeddedFilesNamespace);
        }
        #endregion

        #region 读取文件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subpath"></param>
        /// <returns></returns>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _staticFileProvider.GetDirectoryContents(subpath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subpath"></param>
        /// <returns></returns>
        public IFileInfo GetFileInfo(string subpath)
        {
            return _staticFileProvider.GetFileInfo(subpath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IChangeToken Watch(string filter)
        {
            return _staticFileProvider.Watch(filter);
        }
        #endregion
    }
}
