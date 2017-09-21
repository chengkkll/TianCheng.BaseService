using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class UploadFileController : DataController
    {
        #region 保存一个上传的文件信息
        private long _MaxFileSize = 104857600;  //100M
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
        /// <param name="fileInfo"></param>
        /// <param name="file"></param>
        protected virtual void CheckPostFile(UploadFileInfo fileInfo, Microsoft.AspNetCore.Http.IFormFile file)
        {

        }


        /// <summary>
        /// 保存一个客户端提交的文件
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        protected UploadFileInfo SaveSingleFile(string fullFileName)
        {
            UploadFileInfo result = new UploadFileInfo();
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
            result.FileSize = file.Length;
            if (file.Length > MaxFileSize)
            {
                throw ApiException.BadRequest($"文件太大，只能上传小于{MaxFileSize}个字节的的文件。");
            }

            //设置文件目录
            if (!System.IO.Path.HasExtension(fullFileName))
            {
                fullFileName += System.IO.Path.GetExtension(file.FileName);
            }


            string webpath = AppContext.BaseDirectory;
            if (AppContext.BaseDirectory.Contains("bin\\Debug"))
            {
                webpath = new System.IO.DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            }


            if (fullFileName.Contains("~/"))
            {
                result.WebFileName = fullFileName.Replace("~/", "").Replace("wwwroot", "").Replace("\\", "/").TrimStart('/');
                result.DiskFileName = webpath + fullFileName.Replace("~", "");
            }
            else
            {
                result.WebFileName = fullFileName.Replace(webpath, "").Replace("wwwroot", "").Replace("\\", "/").TrimStart('/');
                result.DiskFileName = fullFileName;
            }
            result.UploadFileName = file.FileName;
            result.MainFileName = System.IO.Path.GetFileNameWithoutExtension(fullFileName);
            result.ExtFileName = System.IO.Path.GetExtension(fullFileName);
            result.SaveDiskDirectory = System.IO.Path.GetDirectoryName(result.DiskFileName);

            //文件格式验证
            CheckPostFile(result, file);

            //设置保存文件的目录
            if (!System.IO.Directory.Exists(result.SaveDiskDirectory))
            {
                System.IO.Directory.CreateDirectory(result.SaveDiskDirectory);
            }

            //保存文件
            using (System.IO.FileStream fs = System.IO.File.Create(result.DiskFileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            return result;
        }


        /// <summary>
        /// 保存客户端提交的多个文件
        /// </summary>
        /// <param name="path">文件目录</param>
        /// <returns></returns>
        protected List<string> SaveSingleFiles(string path)
        {
            List<string> resultList = new List<string>();

            List<Microsoft.AspNetCore.Http.IFormFile> fileList = new List<Microsoft.AspNetCore.Http.IFormFile>();

            foreach (IFormFile file in Request.Form.Files)
            {
                //获取上传的文件
                try
                {
                    string webFileName = "";
                    string diskFileName = "";
                    //文件校验  判断文件大小
                    if (file.Length > MaxFileSize)
                    {
                        throw ApiException.BadRequest($"文件太大，只能上传小于{MaxFileSize}个字节的的文件。");
                    }

                    //文件全名
                    string fullFileName = path + Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);


                    string webpath = AppContext.BaseDirectory;
                    if (AppContext.BaseDirectory.Contains("bin\\Debug"))
                    {
                        webpath = new System.IO.DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
                    }


                    if (fullFileName.Contains("~/"))
                    {
                        webFileName = fullFileName.Replace("~/", "").Replace("wwwroot", "").Replace("\\", "/").TrimStart('/');
                        diskFileName = webpath + fullFileName.Replace("~", "");
                    }
                    else
                    {
                        webFileName = fullFileName.Replace(webpath, "").Replace("wwwroot", "").Replace("\\", "/").TrimStart('/');
                        diskFileName = fullFileName;
                    }

                    string saveDiskDirectory = System.IO.Path.GetDirectoryName(diskFileName);

                    //设置保存文件的目录
                    if (!System.IO.Directory.Exists(saveDiskDirectory))
                    {
                        System.IO.Directory.CreateDirectory(saveDiskDirectory);
                    }

                    //保存文件
                    using (System.IO.FileStream fs = System.IO.File.Create(diskFileName))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    resultList.Add(webFileName);
                }
                catch (Exception ex)
                {
                    throw ApiException.BadRequest(ex.Message);
                }
                if (file == null)
                {
                    throw ApiException.BadRequest("无法获取上传的文件信息");
                }
            }
            return resultList;
        }
        #endregion

    }
}
