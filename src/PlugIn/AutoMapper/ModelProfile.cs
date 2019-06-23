using AutoMapper;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 对象的AutoMapper转换
    /// </summary>
    public class ModelProfile : Profile, IAutoProfile
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ModelProfile()
        {
            Register();
        }
        /// <summary>
        /// 注册需要转换的对象
        /// </summary>
        public void Register()
        {
            // 将上传文件信息转成一个文件路径
            CreateMap<UploadFileInfo, string>().ConvertUsing(new UploadFileInfoToString());
        }
    }

}
