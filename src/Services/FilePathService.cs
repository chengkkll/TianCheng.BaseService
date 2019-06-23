using System;
using TianCheng.Model;

namespace TianCheng.BaseService.Services
{
    /// <summary>
    /// 文件路径操作
    /// </summary>
    public class FilePathService : IServiceRegister
    {
        #region 网站根目录
        static private string _RootPath;
        /// <summary>
        /// 网站根目录
        /// </summary>
        static public string RootPath
        {
            get
            {
                if (String.IsNullOrEmpty(_RootPath))
                {
                    _RootPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot");
                }
                return _RootPath;
            }
        }
        #endregion
    }
}
