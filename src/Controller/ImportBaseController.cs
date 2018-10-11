using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 数据导入的基类处理
    /// </summary>
    public class ImportBaseController : DataController
    {
        #region 保存一个上传的文件信息
        private readonly long _MaxFileSize = 104857600;  //100M
        /// <summary>
        /// 允许文件最大上传的尺寸
        /// </summary>
        protected virtual long MaxFileSize
        {
            get { return _MaxFileSize; }
        }
        /// <summary>
        /// 检查上传的文件
        /// </summary>
        /// <param name="file"></param>
        protected virtual void CheckPostFile(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (System.IO.Path.GetExtension(file.FileName) != ".xlsx")
            {
                throw ApiException.BadRequest("上传的Excel文件格式不对，需要Office2007以上的版本的Excel文件，扩展名为xlsx。");
            }
        }


        /// <summary>
        /// 保存一个客户端提交的文件
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        protected string SaveSingleFile(string fullFileName)
        {
            Microsoft.AspNetCore.Http.IFormFile file;
            //获取上传的文件
            try
            {
                file = Request.Form.Files.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ApiException.BadRequest(ex.Message);
            }
            if (file == null)
            {
                throw ApiException.BadRequest("无法获取上传的文件信息");
            }

            //文件校验  判断文件大小
            if (file.Length > MaxFileSize)
            {
                throw ApiException.BadRequest($"文件太大，只能上传小于{MaxFileSize}个字节的的文件。");
            }
            //文件格式验证
            CheckPostFile(file);

            //设置保存文件的目录
            string filePath = System.IO.Path.GetDirectoryName(fullFileName);

            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }

            //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            //fileName = Guid.NewGuid() + "." + fileName.Split('.')[1];
            //string fileFullName = filePath + fileName;

            using (System.IO.FileStream fs = System.IO.File.Create(fullFileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            return System.IO.Path.GetFileNameWithoutExtension(fullFileName);
        }
        #endregion

        #region 下载文件

        /// <summary>
        /// 下载一个Excel文件
        /// </summary>
        /// <param name="file">文件全路径</param>
        /// <param name="showName">下载时显示的文件名</param>
        /// <returns></returns>
        protected IActionResult DownloadExcel(string file, string showName)
        {
            var extName = System.IO.Path.GetExtension(file);
            if (string.IsNullOrEmpty(showName))
            {
                showName = DateTime.Now.ToString("yyyyMMddhhmmss");
            }
            return PhysicalFile(file, "application/x-xls", $"{showName}.{extName}");
        }
        #endregion
    }
}
