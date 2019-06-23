namespace TianCheng.BaseService
{
    /// <summary>
    /// 上传的文件信息
    /// </summary>
    public class UploadFileInfo
    {
        /// <summary>
        /// 保存后的磁盘路径
        /// </summary>
        public string DiskFileName { get; set; }
        /// <summary>
        /// 保存后的网络路径
        /// </summary>
        public string WebFileName { get; set; }
        /// <summary>
        /// 主文件名
        /// </summary>
        public string MainFileName { get; set; }
        /// <summary>
        /// 扩展名（带.）
        /// </summary>
        public string ExtFileName { get; set; }
        /// <summary>
        /// 上传时的客户端文件名
        /// </summary>
        public string UploadFileName { get; set; }
        /// <summary>
        /// 保存的磁盘目录
        /// </summary>
        public string SaveDiskDirectory { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }


    }
}
