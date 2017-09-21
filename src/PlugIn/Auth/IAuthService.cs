using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace TianCheng.BaseService.PlugIn
{
    /// <summary>
    /// 请求api接口时的权限控制
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 每次请求接口时，加载用户的权限信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="identity"></param>
        void FillFunctionPolicy(string userId, ClaimsIdentity identity);
    }
}
