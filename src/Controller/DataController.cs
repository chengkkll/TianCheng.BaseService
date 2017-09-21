using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TianCheng.BaseService
{
    /// <summary>
    /// 基础数据控制器基类
    /// </summary>
    public class DataController : Controller
    {
        /// <summary>
        /// 获取登录信息
        /// </summary>
        protected TokenLogonInfo LogonInfo
        {
            get
            {
                if (User == null || User.Claims == null)
                {
                    return new TokenLogonInfo();
                }
                TokenLogonInfo info = new TokenLogonInfo();
                foreach (var item in User.Claims)
                {
                    if (item.Type == "name")
                    {
                        info.Name = item.Value;
                    }
                    if (item.Type == "id")
                    {
                        info.Id = item.Value;
                    }
                    if (item.Type == "roleId")
                    {
                        info.RoleId = item.Value;
                    }
                    if (item.Type == "depId")
                    {
                        info.DepartmentId = item.Value;
                    }
                }
                return info;
            }
        }
    }

}
