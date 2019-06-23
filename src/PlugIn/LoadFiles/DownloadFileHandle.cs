using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 下载文件处理
    /// </summary>
    static public class DownloadFileHandle
    {
        #region 下载Excel文件
        /// <summary>
        /// 下载一个Excel文件
        /// </summary>
        /// <param name="controller">当前请求的Controller</param>
        /// <param name="file">文件全路径</param>
        /// <param name="showName">下载时显示的文件名</param>
        /// <returns></returns>
        static public IActionResult DownloadExcel(this Controller controller, string file, string showName = "")
        {
            var extName = System.IO.Path.GetExtension(file);
            if (string.IsNullOrEmpty(showName))
            {
                showName = DateTime.Now.ToString("yyyyMMddhhmmss");
            }
            return controller.PhysicalFile(file, "application/x-xls", $"{showName}.{extName}");
        }
        #endregion
    }
}
