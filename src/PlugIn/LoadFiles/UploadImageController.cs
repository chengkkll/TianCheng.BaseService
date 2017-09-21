using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 图片上传
    /// </summary>
    public class UploadImageController : UploadFileController
    {
        /// <summary>
        /// 检查上传的文件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="file"></param>
        protected override void CheckPostFile(UploadFileInfo fileInfo, IFormFile file)
        {
            string ext = System.IO.Path.GetExtension(file.FileName);
            //Todo ： 先简单判断扩展名，有时间分析下图片格式。
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".bmp")
            {
                throw ApiException.BadRequest("图片格式不对");
            }
        }
    }
}
