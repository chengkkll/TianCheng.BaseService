using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// 属性/类别的说明特性
    /// </summary>
    public class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="desc"></param>
        public DescriptionAttribute(string desc)
        {
            Desc = desc;
        }
        /// <summary>
        /// 添加属性/类别的说明
        /// </summary>
        /// <param name="desc">说明</param>
        /// <param name="remark">备注一些特殊情况，比如只读等</param>
        public DescriptionAttribute(string desc, RemarkType remark)
        {
            Desc = desc;

            if ((remark & RemarkType.Required) == RemarkType.Required)
            {
                Remark += " 必填   ";
            }
            if ((remark & RemarkType.NoRepeat) == RemarkType.NoRepeat)
            {
                Remark += " 不能重复   ";
            }
        }
        /// <summary>
        /// 添加属性/类别的说明
        /// </summary>
        /// <param name="desc">说明</param>
        /// <param name="remark">备注一些特殊情况，比如只读等</param>
        public DescriptionAttribute(string desc, string remark)
        {
            Desc = desc;
            Remark = remark;
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 备注的类型
    /// </summary>
    public enum RemarkType
    {
        /// <summary>
        /// 必填
        /// </summary>
        Required = 1,
        /// <summary>
        /// 不能重复
        /// </summary>
        NoRepeat = 2,
    }
}
