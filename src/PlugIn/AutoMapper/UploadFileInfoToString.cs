using AutoMapper;


namespace TianCheng.BaseService
{
    /// <summary>
    /// UploadFileInfo? -> String
    /// </summary>
    public class UploadFileInfoToString : ITypeConverter<UploadFileInfo, string>
    {
        /// <summary>
        ///  UploadFileInfo? -> String
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Convert(UploadFileInfo source, string destination, ResolutionContext context)
        {
            return source.WebFileName;
        }
    }
}
