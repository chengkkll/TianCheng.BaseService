using System;
using System.Collections.Generic;
using System.Linq;

namespace TianCheng.BaseService.PlugIn.Swagger
{
    /// <summary>
    /// 对接口进行排序
    /// </summary>
    public class SwaggerOrderBy
    {
        static Dictionary<string, string> _OrderDict = null;
        static Dictionary<string, string> OrderDict
        {
            get
            {
                if (_OrderDict == null)
                {
                    _OrderDict = SwaggerDocInfo.ModelIndex();
                }
                return _OrderDict;
            }
        }
        static private int index = 0;
        /// <summary>
        /// 对接口进行排序
        /// </summary>
        /// <param name="des"></param>
        /// <returns>
        /// 排序规则
        /// 1、按接口所在模块排序（根据配置文件所设置）
        /// 2、根据接口在Controller中代码中的顺序。
        /// </returns>
        static public string Order(Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription des)
        {
            if (des.ActionDescriptor != null && !String.IsNullOrWhiteSpace(des.ActionDescriptor.DisplayName))
            {
                string ctrl;
                try
                {
                    var desc = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)des.ActionDescriptor;
                    ctrl = desc.ControllerTypeInfo.FullName;
                }
                catch
                {
                    ctrl = des.ActionDescriptor.DisplayName;
                }

                string key = OrderDict.Keys.Where(e => ctrl.Contains(e)).FirstOrDefault();
                string module;
                if (key != null && OrderDict.ContainsKey(key))
                {
                    module = OrderDict[key];
                }
                else
                {
                    module = "xx";
                }
                return module + (index++).ToString();
            }
            return des.RelativePath;
        }
    }
}
