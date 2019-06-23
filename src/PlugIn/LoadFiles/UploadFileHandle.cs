using Microsoft.AspNetCore.Http;
using System;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public class UploadFileHandle
    {
        #region 构造方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxFileSize">允许文件最大上传的尺寸</param>
        public UploadFileHandle(long maxFileSize)
        {
            MaxFileSize = maxFileSize;
        }
        /// <summary>
        /// 
        /// </summary>
        public UploadFileHandle()
        {

        }
        #endregion

        #region 保存一个上传的文件信息
        /// <summary>
        /// 允许文件最大上传的尺寸
        /// </summary>
        public long MaxFileSize { get; set; } = 104857600;  //100M

        /// <summary>
        /// 获取存放文件的根目录
        /// </summary>
        static private string DiskPath
        {
            get
            {
                string webpath = AppContext.BaseDirectory;
                if (AppContext.BaseDirectory.Contains("bin\\Debug"))
                {
                    webpath = new System.IO.DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
                }
                return webpath;
            }
        }

        /// <summary>
        /// 保存一个客户端提交的文件
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <param name="file"></param>
        /// <param name="checkFile"></param>
        /// <returns></returns>
        protected UploadFileInfo SaveSingleFile(string fullFileName, IFormFile file, Action<UploadFileInfo, IFormFile> checkFile)
        {
            // 1、检验参数
            // 1.1、获取上传的文件
            if (file == null)
            {
                throw ApiException.BadRequest("上传的文件信息为空");
            }
            // 1.2、文件校验  判断文件大小
            if (file.Length > MaxFileSize)
            {
                throw ApiException.BadRequest($"文件太大，只能上传小于{MaxFileSize}个字节的的文件。");
            }

            // 2、完善数据
            // 2.1、如果文件没有扩展名，根据上传的文件设置
            if (!System.IO.Path.HasExtension(fullFileName))
            {
                fullFileName += System.IO.Path.GetExtension(file.FileName);
            }
            // 2.2、设置上传文件信息
            UploadFileInfo result = new UploadFileInfo
            {
                FileSize = file.Length,
                WebFileName = fullFileName.Replace("~/", "").Replace(DiskPath, "").Replace("wwwroot", "").Replace("\\", "/").TrimStart('/'),
                DiskFileName = fullFileName.Contains("~/") ? DiskPath + fullFileName.Replace("~", "") : fullFileName,
                UploadFileName = file.FileName,
                MainFileName = System.IO.Path.GetFileNameWithoutExtension(fullFileName),
                ExtFileName = System.IO.Path.GetExtension(fullFileName)
            };
            result.SaveDiskDirectory = System.IO.Path.GetDirectoryName(result.DiskFileName);

            // 3、文件格式验证
            if (checkFile != null)
            {
                checkFile.Invoke(result, file);
            }

            // 4、保存文件
            // 4.1、设置保存文件的目录
            if (!System.IO.Directory.Exists(result.SaveDiskDirectory))
            {
                System.IO.Directory.CreateDirectory(result.SaveDiskDirectory);
            }
            // 4.2、保存文件
            using (System.IO.FileStream fs = System.IO.File.Create(result.DiskFileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            //5、返回上传成功的文件信息
            return result;
        }
        #endregion

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        static public void Delete(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }
            try
            {
                string file = System.IO.Path.Combine(DiskPath, "wwwroot", fileName);
                System.IO.File.Delete(file);
            }
            catch { }
        }

        /// <summary>
        /// 上传图片操作
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <param name="file"></param>
        static public UploadFileInfo UploadImage(string fullFileName, IFormFile file)
        {
            // 控制上传文件不能大于5M
            UploadFileHandle handle = new UploadFileHandle(1024 * 1024 * 5);
            return handle.SaveSingleFile(fullFileName, file, (info, f) =>
            {
                // 获取扩展名
                string ext = System.IO.Path.GetExtension(f.FileName);
                //Todo ： 先简单判断扩展名，有时间分析下图片格式。
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".bmp")
                {
                    throw ApiException.BadRequest("图片格式不对");
                }
            });
        }

        /// <summary>
        /// 上传Excel文件
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        static public UploadFileInfo UploadExcel(string fullFileName, IFormFile file)
        {
            // 控制上传文件不能大于50M
            UploadFileHandle handle = new UploadFileHandle(1024 * 1024 * 50);
            return handle.SaveSingleFile(fullFileName, file, (info, f) =>
            {
                // 获取扩展名
                string ext = System.IO.Path.GetExtension(f.FileName);
                //Todo ： 先简单判断扩展名。
                if (ext != ".xlsx")
                {
                    throw ApiException.BadRequest("上传的Excel文件格式不对，需要Office2007以上的版本的Excel文件，扩展名为xlsx。");
                }
            });
        }
    }
}
