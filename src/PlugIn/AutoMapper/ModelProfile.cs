using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TianCheng.Model;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 基础信息的AutoMapper转换
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
        /// 
        /// </summary>
        public void Register()
        {
            CreateMap<UploadFileInfo, string>().ConvertUsing(new UploadFileInfoToString());
        }
    }

}
